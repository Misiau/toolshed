using ToolShed;

namespace ToolShed.Tests;

public class LoanServiceTests
{
    private readonly InMemoryLoanRepository _loans = new();
    private readonly FixedClock _clock = new(TestData.Monday);

    private LoanService Service(params Customer[] customers)
    {
        var items = new InMemoryItemRepository(new[] { TestData.Drill() });
        var customerRepo = new InMemoryCustomerRepository(customers.Length > 0 ? customers : new[] { TestData.Standard(), TestData.Member() });
        var availability = new AvailabilityService(_loans, new InMemoryReservationRepository());
        return new LoanService(items, customerRepo, _loans, availability, new PricingService(), _clock);
    }

    [Fact]
    public void Rent_StandardCustomer_StoresPriceAndDeposit()
    {
        var loan = Service().Rent(customerId: 2, itemId: 1, TestData.Monday, TestData.Monday.AddDays(2));

        Assert.Equal(60m, loan.TotalPrice);
        Assert.Equal(100m, loan.Deposit); // max(100, 20% z 450 = 90) = 100
        Assert.Equal(LoanStatus.Active, loan.Status);
    }

    [Fact]
    public void Rent_Member_NoDeposit()
    {
        var loan = Service().Rent(customerId: 1, itemId: 1, TestData.Monday, TestData.Monday.AddDays(2));

        Assert.Equal(0m, loan.Deposit);
    }

    [Fact]
    public void Rent_LongerThan30Days_Rejected_Rent13()
    {
        var ex = Assert.Throws<LoanException>(() =>
            Service().Rent(2, 1, TestData.Monday, TestData.Monday.AddDays(30)));

        Assert.Equal("RENT-13", ex.RuleId);
    }

    [Fact]
    public void Rent_BlockedCustomer_Rejected_Rent70()
    {
        var blocked = new Customer { Id = 9, Name = "Marek", Email = "marek@example.com", IsBlocked = true };

        var ex = Assert.Throws<LoanException>(() =>
            Service(blocked).Rent(9, 1, TestData.Monday, TestData.Monday));

        Assert.Equal("RENT-70", ex.RuleId);
    }

    [Fact]
    public void Return_ClosesLoan()
    {
        var service = Service();
        var loan = service.Rent(2, 1, TestData.Monday, TestData.Monday.AddDays(2));

        var returned = service.Return(loan.Id, TestData.Monday.AddDays(2));

        Assert.Equal(LoanStatus.Returned, returned.Status);
        Assert.Equal(TestData.Monday.AddDays(2), returned.ReturnedOn);
    }
}

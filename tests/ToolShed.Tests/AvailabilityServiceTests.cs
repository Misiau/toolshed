using ToolShed;

namespace ToolShed.Tests;

public class AvailabilityServiceTests
{
    private readonly InMemoryLoanRepository _loans = new();
    private readonly InMemoryReservationRepository _reservations = new();

    private AvailabilityService Service() => new(_loans, _reservations);

    [Fact]
    public void FreeItem_IsAvailable()
    {
        Assert.True(Service().IsAvailable(TestData.Drill(), TestData.Monday, TestData.Monday.AddDays(3)));
    }

    [Fact]
    public void ActiveLoanOverlapping_NotAvailable()
    {
        var item = TestData.Drill();
        _loans.Add(new Loan { ItemId = item.Id, PickupDate = TestData.Monday, DueDate = TestData.Monday.AddDays(4) });

        Assert.False(Service().IsAvailable(item, TestData.Monday.AddDays(2), TestData.Monday.AddDays(6)));
    }

    [Fact]
    public void PickupOnReturnDay_NotAvailable_Rent51()
    {
        // RENT-51: dzień zwrotu jest dniem przeglądu, odbiór tego samego dnia to konflikt
        var item = TestData.Drill();
        _loans.Add(new Loan { ItemId = item.Id, PickupDate = TestData.Monday, DueDate = TestData.Monday.AddDays(4) });

        Assert.False(Service().IsAvailable(item, TestData.Monday.AddDays(4), TestData.Monday.AddDays(5)));
        Assert.True(Service().IsAvailable(item, TestData.Monday.AddDays(5), TestData.Monday.AddDays(6)));
    }

    [Fact]
    public void ItemInRepair_NotAvailable_Rent52()
    {
        var item = TestData.Drill();
        item.Condition = ItemCondition.InRepair;

        Assert.False(Service().IsAvailable(item, TestData.Monday, TestData.Monday));
    }
}

namespace ToolShed;

public class LoanException : Exception
{
    public LoanException(string ruleId, string message) : base($"{ruleId}: {message}") => RuleId = ruleId;

    public string RuleId { get; }
}

/// <summary>
/// Wypożyczenia i zwroty. Pilnuje reguł:
/// RENT-13 maksymalnie 30 dni, odbiór nie w przeszłości;
/// RENT-50 dostępność; RENT-70 klient zablokowany albo z zaległym zwrotem nie wypożycza.
/// Ceny: PricingService, kaucja: DepositPolicy, spóźnienie: LateFee.
/// </summary>
public class LoanService
{
    public const int MaxLoanDays = 30;

    private readonly IItemRepository _items;
    private readonly ICustomerRepository _customers;
    private readonly ILoanRepository _loans;
    private readonly AvailabilityService _availability;
    private readonly PricingService _pricing;
    private readonly IClock _clock;

    public LoanService(
        IItemRepository items,
        ICustomerRepository customers,
        ILoanRepository loans,
        AvailabilityService availability,
        PricingService pricing,
        IClock clock)
    {
        _items = items;
        _customers = customers;
        _loans = loans;
        _availability = availability;
        _pricing = pricing;
        _clock = clock;
    }

    public Loan Rent(int customerId, int itemId, DateOnly from, DateOnly to)
    {
        var customer = _customers.Get(customerId) ?? throw new LoanException("RENT-70", "Nie ma takiego klienta.");
        var item = _items.Get(itemId) ?? throw new LoanException("RENT-50", "Nie ma takiego sprzętu.");

        if (from < _clock.Today)
        {
            throw new LoanException("RENT-13", "Odbiór nie może być w przeszłości.");
        }

        if (to.DayNumber - from.DayNumber + 1 > MaxLoanDays)
        {
            throw new LoanException("RENT-13", $"Maksymalny okres wypożyczenia to {MaxLoanDays} dni.");
        }

        if (customer.IsBlocked || HasOverdueLoan(customer.Id))
        {
            throw new LoanException("RENT-70", "Klient nie może teraz wypożyczać.");
        }

        if (!_availability.IsAvailable(item, from, to))
        {
            throw new LoanException("RENT-50", "Sprzęt jest niedostępny w tym terminie.");
        }

        var quote = _pricing.Quote(item, customer, from, to);

        return _loans.Add(new Loan
        {
            ItemId = item.Id,
            CustomerId = customer.Id,
            ItemName = item.Name,
            PickupDate = from,
            DueDate = to,
            TotalPrice = quote.Total,
            Deposit = DepositPolicy.For(item, customer),
        });
    }

    public Loan Return(int loanId, DateOnly returnedOn)
    {
        var loan = _loans.Get(loanId) ?? throw new LoanException("RENT-20", "Nie ma takiego wypożyczenia.");
        if (loan.Status != LoanStatus.Active)
        {
            throw new LoanException("RENT-20", "To wypożyczenie jest już zamknięte.");
        }

        loan.ReturnedOn = returnedOn;
        loan.Status = LoanStatus.Returned;
        loan.LateFeeCharged = LateFee.Calculate(loan, returnedOn);
        return loan;
    }

    private bool HasOverdueLoan(int customerId)
        => _loans.ForCustomer(customerId).Any(l => l.IsOverdue(_clock.Today));
}

namespace ToolShed;

/// <summary>Repozytoria w pamięci: wystarczą do testów i do warsztatu. Bez bazy danych.</summary>
public class InMemoryItemRepository : IItemRepository
{
    private readonly List<Item> _items;

    public InMemoryItemRepository(IEnumerable<Item> items) => _items = items.ToList();

    public Item? Get(int id) => _items.FirstOrDefault(i => i.Id == id);

    public IReadOnlyList<Item> All() => _items;
}

public class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers;

    public InMemoryCustomerRepository(IEnumerable<Customer> customers) => _customers = customers.ToList();

    public Customer? Get(int id) => _customers.FirstOrDefault(c => c.Id == id);
}

public class InMemoryLoanRepository : ILoanRepository
{
    private readonly List<Loan> _loans = new();
    private int _nextId = 1;

    public Loan? Get(int id) => _loans.FirstOrDefault(l => l.Id == id);

    public IReadOnlyList<Loan> All() => _loans;

    public IEnumerable<Loan> ForItem(int itemId) => _loans.Where(l => l.ItemId == itemId);

    public IEnumerable<Loan> ForCustomer(int customerId) => _loans.Where(l => l.CustomerId == customerId);

    public Loan Add(Loan loan)
    {
        var stored = new Loan
        {
            Id = _nextId++,
            ItemId = loan.ItemId,
            CustomerId = loan.CustomerId,
            ItemName = loan.ItemName,
            PickupDate = loan.PickupDate,
            DueDate = loan.DueDate,
            TotalPrice = loan.TotalPrice,
            Deposit = loan.Deposit,
        };
        _loans.Add(stored);
        return stored;
    }
}

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly List<Reservation> _reservations = new();
    private int _nextId = 1;

    public IEnumerable<Reservation> ForItem(int itemId) => _reservations.Where(r => r.ItemId == itemId);

    public Reservation Add(Reservation reservation)
    {
        var stored = new Reservation
        {
            Id = _nextId++,
            ItemId = reservation.ItemId,
            CustomerId = reservation.CustomerId,
            From = reservation.From,
            To = reservation.To,
        };
        _reservations.Add(stored);
        return stored;
    }
}

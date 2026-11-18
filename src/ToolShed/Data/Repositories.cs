namespace ToolShed;

public interface IItemRepository
{
    Item? Get(int id);
    IReadOnlyList<Item> All();
}

public interface ICustomerRepository
{
    Customer? Get(int id);
}

public interface ILoanRepository
{
    Loan? Get(int id);
    IReadOnlyList<Loan> All();
    IEnumerable<Loan> ForItem(int itemId);
    IEnumerable<Loan> ForCustomer(int customerId);
    Loan Add(Loan loan);
}

public interface IReservationRepository
{
    IEnumerable<Reservation> ForItem(int itemId);
    Reservation Add(Reservation reservation);
}

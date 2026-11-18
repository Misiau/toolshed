namespace ToolShed;

public record CategoryRevenue(ItemCategory Category, int Loans, decimal Rental, decimal LateFees)
{
    public decimal Total => Rental + LateFees;
}

/// <summary>
/// RENT-80: raport miesięczny dla właściciela. Przychód liczymy z wypożyczeń ZWRÓCONYCH w danym miesiącu
/// (data zwrotu, nie odbioru), osobno opłata za wypożyczenie i opłaty za spóźnienie. Kaucje to nie przychód.
/// </summary>
public class MonthlyRevenueReport
{
    private readonly ILoanRepository _loans;
    private readonly IItemRepository _items;

    public MonthlyRevenueReport(ILoanRepository loans, IItemRepository items)
    {
        _loans = loans;
        _items = items;
    }

    public IReadOnlyList<CategoryRevenue> For(int year, int month)
    {
        return _loans.All()
            .Where(l => l.Status == LoanStatus.Returned
                        && l.ReturnedOn is { } r && r.Year == year && r.Month == month)
            .GroupBy(l => _items.Get(l.ItemId)?.Category ?? ItemCategory.PowerTools)
            .Select(g => new CategoryRevenue(g.Key, g.Count(), g.Sum(l => l.TotalPrice), g.Sum(l => l.LateFeeCharged)))
            .OrderByDescending(c => c.Total)
            .ToList();
    }
}

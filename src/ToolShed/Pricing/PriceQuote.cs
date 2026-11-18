namespace ToolShed;

/// <summary>Wycena wypożyczenia: wszystkie kwoty w złotych, zaokrąglone do groszy.</summary>
public record PriceQuote(
    int Days,
    int WeekendDays,
    decimal BasePrice,
    decimal WeekendDiscount,
    decimal MemberDiscount,
    decimal Total);

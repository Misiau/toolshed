namespace ToolShed;

/// <summary>Egzemplarz sprzętu do wypożyczenia (jedna sztuka, nie typ produktu).</summary>
public class Item
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public ItemCategory Category { get; init; }

    /// <summary>Stawka za jeden dzień wypożyczenia, w złotych (RENT-30).</summary>
    public decimal DailyRate { get; init; }

    /// <summary>Wartość odtworzeniowa egzemplarza, podstawa kaucji (RENT-40).</summary>
    public decimal ReplacementValue { get; init; }

    /// <summary>Minimalna kaucja ustalona dla egzemplarza (RENT-40).</summary>
    public decimal MinimumDeposit { get; init; }

    public ItemCondition Condition { get; set; } = ItemCondition.Good;

    /// <summary>RENT-52: sprzęt w naprawie nie może być wypożyczony ani zarezerwowany.</summary>
    public bool IsRentable => Condition != ItemCondition.InRepair;
}

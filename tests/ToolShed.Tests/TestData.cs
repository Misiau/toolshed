using ToolShed;

namespace ToolShed.Tests;

/// <summary>Wspólne dane do testów. 2 marca 2026 to poniedziałek.</summary>
internal static class TestData
{
    public static readonly DateOnly Monday = new(2026, 3, 2);

    public static Item Drill(decimal dailyRate = 20m) => new()
    {
        Id = 1,
        Name = "Wiertarka",
        Category = ItemCategory.PowerTools,
        DailyRate = dailyRate,
        ReplacementValue = 450m,
        MinimumDeposit = 100m,
    };

    public static Customer Standard() => new() { Id = 2, Name = "Piotr", Email = "piotr@example.com" };

    public static Customer Member() => new() { Id = 1, Name = "Anna", Email = "anna@example.com", Tier = CustomerTier.Member };
}

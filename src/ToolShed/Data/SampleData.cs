namespace ToolShed;

/// <summary>Przykładowy magazyn i klienci. Dane wymyślone.</summary>
public static class SampleData
{
    public static IReadOnlyList<Item> Items { get; } = new List<Item>
    {
        new() { Id = 1, Name = "Wiertarka udarowa", Category = ItemCategory.PowerTools, DailyRate = 25m, ReplacementValue = 450m, MinimumDeposit = 100m },
        new() { Id = 2, Name = "Szlifierka kątowa", Category = ItemCategory.PowerTools, DailyRate = 20m, ReplacementValue = 320m, MinimumDeposit = 80m },
        new() { Id = 3, Name = "Kosiarka spalinowa", Category = ItemCategory.Garden, DailyRate = 60m, ReplacementValue = 1800m, MinimumDeposit = 200m },
        new() { Id = 4, Name = "Nożyce do żywopłotu", Category = ItemCategory.Garden, DailyRate = 30m, ReplacementValue = 600m, MinimumDeposit = 100m },
        new() { Id = 5, Name = "Drabina 3-częściowa", Category = ItemCategory.Ladders, DailyRate = 15m, ReplacementValue = 700m, MinimumDeposit = 50m },
        new() { Id = 6, Name = "Myjka ciśnieniowa", Category = ItemCategory.Cleaning, DailyRate = 45m, ReplacementValue = 1200m, MinimumDeposit = 150m },
        new() { Id = 7, Name = "Odkurzacz przemysłowy", Category = ItemCategory.Cleaning, DailyRate = 35m, ReplacementValue = 900m, MinimumDeposit = 100m, Condition = ItemCondition.InRepair },
        new() { Id = 8, Name = "Namiot 4-osobowy", Category = ItemCategory.Camping, DailyRate = 18m, ReplacementValue = 500m, MinimumDeposit = 50m },
    };

    public static IReadOnlyList<Customer> Customers { get; } = new List<Customer>
    {
        new() { Id = 1, Name = "Anna Wiśniewska", Email = "anna@example.com", Tier = CustomerTier.Member },
        new() { Id = 2, Name = "Piotr Zieliński", Email = "piotr@example.com" },
        new() { Id = 3, Name = "Kasia Nowak", Email = "kasia@example.com", Tier = CustomerTier.Member },
        new() { Id = 4, Name = "Marek Lewandowski", Email = "marek@example.com", IsBlocked = true },
    };
}

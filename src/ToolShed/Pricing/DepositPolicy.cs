namespace ToolShed;

/// <summary>
/// RENT-40: kaucja.
/// - członkowie klubu nie płacą kaucji,
/// - pozostali: większa z kwot: minimalna kaucja egzemplarza albo 20% wartości odtworzeniowej,
/// - kaucja zaokrąglona w górę do pełnych 10 zł.
/// </summary>
public static class DepositPolicy
{
    public const decimal ReplacementValueShare = 0.20m;

    public static decimal For(Item item, Customer customer)
    {
        if (customer.IsMember)
        {
            return 0m;
        }

        var deposit = Math.Max(item.MinimumDeposit, item.ReplacementValue * ReplacementValueShare);
        return Math.Ceiling(deposit / 10m) * 10m;
    }
}

namespace ToolShed;

public class Customer
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";
    public CustomerTier Tier { get; init; } = CustomerTier.Standard;

    /// <summary>RENT-70: klient zablokowany ręcznie przez obsługę (np. zniszczony sprzęt).</summary>
    public bool IsBlocked { get; set; }

    public bool IsMember => Tier == CustomerTier.Member;
}

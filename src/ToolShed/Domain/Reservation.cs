namespace ToolShed;

/// <summary>Rezerwacja egzemplarza na przyszłe dni (RENT-50). Zamienia się w wypożyczenie przy odbiorze.</summary>
public class Reservation
{
    public int Id { get; init; }
    public int ItemId { get; init; }
    public int CustomerId { get; init; }
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }
    public bool IsCancelled { get; set; }
}

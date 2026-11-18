namespace ToolShed;

/// <summary>
/// RENT-50: egzemplarz nie może być w dwóch miejscach naraz. Okresy porównujemy włącznie z oboma końcami,
/// więc zwrot i kolejny odbiór tego samego dnia to konflikt (RENT-51: dzień na przegląd).
/// RENT-52: sprzęt w naprawie jest niedostępny.
/// </summary>
public class AvailabilityService
{
    private readonly ILoanRepository _loans;
    private readonly IReservationRepository _reservations;

    public AvailabilityService(ILoanRepository loans, IReservationRepository reservations)
    {
        _loans = loans;
        _reservations = reservations;
    }

    public bool IsAvailable(Item item, DateOnly from, DateOnly to, int? ignoreReservationId = null)
    {
        if (!item.IsRentable)
        {
            return false;
        }

        var loanConflict = _loans.ForItem(item.Id)
            .Where(l => l.Status == LoanStatus.Active)
            .Any(l => Overlaps(l.PickupDate, l.DueDate, from, to));

        var reservationConflict = _reservations.ForItem(item.Id)
            .Where(r => !r.IsCancelled && r.Id != ignoreReservationId)
            .Any(r => Overlaps(r.From, r.To, from, to));

        return !loanConflict && !reservationConflict;
    }

    private static bool Overlaps(DateOnly aFrom, DateOnly aTo, DateOnly bFrom, DateOnly bTo)
        => aFrom <= bTo && bFrom <= aTo;
}

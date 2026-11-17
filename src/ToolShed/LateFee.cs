namespace ToolShed;

/// <summary>
/// RENT-20: opłata za spóźnienie.
/// - zwrot w dniu terminu albo wcześniej: 0 zł,
/// - pierwszy dzień spóźnienia jest darmowy (karencja),
/// - każdy kolejny dzień: 5 zł,
/// - opłata nigdy nie przekracza 50 zł.
/// </summary>
public static class LateFee
{
    public const decimal PerDay = 5m;
    public const decimal Cap = 50m;
    public const int GraceDays = 1;

    public static decimal Calculate(Loan loan, DateOnly returnedOn)
    {
        var daysLate = returnedOn.DayNumber - loan.DueDate.DayNumber;
        if (daysLate <= GraceDays)
        {
            return 0m;
        }

        var fee = (daysLate - GraceDays) * PerDay;
        return Math.Min(fee, Cap);
    }
}

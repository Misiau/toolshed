namespace ToolShed;

/// <summary>
/// Cennik wypożyczeń. Reguły w docs/regulamin.md:
/// RENT-30 stawka dzienna, dni weekendowe 30% taniej;
/// RENT-31 członkowie klubu: dodatkowe 10% od kwoty po rabacie weekendowym;
/// RENT-32 minimalna opłata za wypożyczenie: 15 zł.
/// </summary>
public class PricingService
{
    public const decimal WeekendDiscountRate = 0.30m;
    public const decimal MemberDiscountRate = 0.10m;
    public const decimal MinimumCharge = 15m;

    public PriceQuote Quote(Item item, Customer customer, DateOnly from, DateOnly to)
    {
        if (to < from)
        {
            throw new ArgumentException("Data zwrotu nie może być wcześniejsza niż data odbioru.", nameof(to));
        }

        // RENT-12: oba końce okresu się liczą.
        var days = to.DayNumber - from.DayNumber + 1;
        var weekendDays = CountWeekendDays(from, to);

        var basePrice = days * item.DailyRate;
        var weekendDiscount = weekendDays * item.DailyRate * WeekendDiscountRate;
        var afterWeekend = basePrice - weekendDiscount;

        var memberDiscount = customer.IsMember ? afterWeekend * MemberDiscountRate : 0m;
        var total = Math.Max(afterWeekend - memberDiscount, MinimumCharge);

        return new PriceQuote(
            days,
            weekendDays,
            Round(basePrice),
            Round(weekendDiscount),
            Round(memberDiscount),
            Round(total));
    }

    private static int CountWeekendDays(DateOnly from, DateOnly to)
    {
        var count = 0;
        for (var day = from; day < to; day = day.AddDays(1))
        {
            if (day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                count++;
            }
        }

        return count;
    }

    private static decimal Round(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);
}

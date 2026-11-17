using ToolShed;

namespace ToolShed.Tests;

public class LoanTests
{
    [Fact]
    public void DurationDays_CountsPickupAndReturnDay()
    {
        // RENT-12: 2-8 marca to 7 dni (oba końce się liczą).
        var loan = new Loan
        {
            ItemName = "Wiertarka",
            PickupDate = new DateOnly(2026, 3, 2),
            DueDate = new DateOnly(2026, 3, 8),
        };

        Assert.Equal(7, loan.DurationDays);
    }

    [Fact]
    public void IsOverdue_FalseOnDueDate_TrueDayAfter()
    {
        var loan = new Loan
        {
            ItemName = "Szlifierka",
            PickupDate = new DateOnly(2026, 3, 2),
            DueDate = new DateOnly(2026, 3, 8),
        };

        Assert.False(loan.IsOverdue(new DateOnly(2026, 3, 8)));
        Assert.True(loan.IsOverdue(new DateOnly(2026, 3, 9)));
    }
}

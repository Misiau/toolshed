using ToolShed;

namespace ToolShed.Tests;

public class PricingServiceTests
{
    private readonly PricingService _pricing = new();

    [Fact]
    public void Quote_WeekdaysOnly_DaysTimesRate()
    {
        // poniedziałek-środa: 3 dni po 20 zł
        var quote = _pricing.Quote(TestData.Drill(), TestData.Standard(), TestData.Monday, TestData.Monday.AddDays(2));

        Assert.Equal(3, quote.Days);
        Assert.Equal(0, quote.WeekendDays);
        Assert.Equal(60m, quote.Total);
    }

    [Fact]
    public void Quote_WeekendInsidePeriod_ThirtyPercentOffWeekendDays()
    {
        // piątek-poniedziałek: 4 dni, sobota i niedziela w środku
        var friday = TestData.Monday.AddDays(4);
        var quote = _pricing.Quote(TestData.Drill(), TestData.Standard(), friday, friday.AddDays(3));

        Assert.Equal(4, quote.Days);
        Assert.Equal(2, quote.WeekendDays);
        Assert.Equal(80m, quote.BasePrice);
        Assert.Equal(12m, quote.WeekendDiscount);
        Assert.Equal(68m, quote.Total);
    }

    [Fact]
    public void Quote_Member_TenPercentAfterWeekendDiscount()
    {
        var quote = _pricing.Quote(TestData.Drill(), TestData.Member(), TestData.Monday, TestData.Monday.AddDays(2));

        Assert.Equal(6m, quote.MemberDiscount);
        Assert.Equal(54m, quote.Total);
    }

    [Fact]
    public void Quote_CheapShortLoan_MinimumCharge()
    {
        var quote = _pricing.Quote(TestData.Drill(dailyRate: 10m), TestData.Standard(), TestData.Monday, TestData.Monday);

        Assert.Equal(PricingService.MinimumCharge, quote.Total);
    }

    [Fact]
    public void Quote_ReturnBeforePickup_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            _pricing.Quote(TestData.Drill(), TestData.Standard(), TestData.Monday, TestData.Monday.AddDays(-1)));
    }
}

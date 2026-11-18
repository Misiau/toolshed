using System.Text.Json.Serialization;
using ToolShed;

// ToolShed.Api: cienka warstwa HTTP nad biblioteką ToolShed. Cała logika i reguły RENT-xx są w src/ToolShed.
// Dane w pamięci (SampleData + kilka wypożyczeń startowych): po restarcie wszystko wraca do stanu początkowego.

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddSingleton<IItemRepository>(_ => new InMemoryItemRepository(SampleData.Items));
builder.Services.AddSingleton<ICustomerRepository>(_ => new InMemoryCustomerRepository(SampleData.Customers));
builder.Services.AddSingleton<ILoanRepository, InMemoryLoanRepository>();
builder.Services.AddSingleton<IReservationRepository, InMemoryReservationRepository>();
builder.Services.AddSingleton<INotificationSender, ConsoleNotificationSender>();
builder.Services.AddSingleton<AvailabilityService>();
builder.Services.AddSingleton<PricingService>();
builder.Services.AddSingleton<LoanService>();
builder.Services.AddSingleton<OverdueReminderJob>();
builder.Services.AddSingleton<MonthlyRevenueReport>();

var app = builder.Build();

StartupData.Seed(app.Services.GetRequiredService<ILoanRepository>(), app.Services.GetRequiredService<IClock>());

app.MapGet("/", () => Results.Text("ToolShed API. Zobacz requests.http w katalogu projektu."));

// --- Sprzęt i wyceny ------------------------------------------------------------------------
app.MapGet("/items", (IItemRepository items) => items.All());

app.MapGet("/items/{id:int}/availability", (int id, DateOnly from, DateOnly to, IItemRepository items, AvailabilityService availability) =>
    items.Get(id) is { } item
        ? Results.Ok(new { item.Id, item.Name, from, to, available = availability.IsAvailable(item, from, to) })
        : Results.NotFound());

app.MapGet("/items/{id:int}/quote", (int id, int customerId, DateOnly from, DateOnly to,
    IItemRepository items, ICustomerRepository customers, PricingService pricing) =>
{
    var item = items.Get(id);
    var customer = customers.Get(customerId);
    if (item is null || customer is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new { quote = pricing.Quote(item, customer, from, to), deposit = DepositPolicy.For(item, customer) });
});

// --- Wypożyczenia ---------------------------------------------------------------------------
app.MapGet("/loans", (ILoanRepository loans) => loans.All());

app.MapPost("/loans", (RentRequest request, LoanService service) =>
    Handle(() => service.Rent(request.CustomerId, request.ItemId, request.From, request.To)));

app.MapPost("/loans/{id:int}/return", (int id, ReturnRequest request, LoanService service) =>
    Handle(() => service.Return(id, request.ReturnedOn)));

// --- Przypomnienia i raport -----------------------------------------------------------------
app.MapPost("/jobs/overdue-reminders", (OverdueReminderJob job) => Results.Ok(new { sent = job.Run() }));

app.MapGet("/reports/monthly", (int year, int month, MonthlyRevenueReport report) => report.For(year, month));

app.Run();

// Naruszenie reguły biznesowej → 400 z identyfikatorem reguły.
static IResult Handle(Func<Loan> action)
{
    try
    {
        return Results.Ok(action());
    }
    catch (LoanException ex)
    {
        return Results.BadRequest(new { rule = ex.RuleId, error = ex.Message });
    }
}

public record RentRequest(int CustomerId, int ItemId, DateOnly From, DateOnly To);

public record ReturnRequest(DateOnly ReturnedOn);

/// <summary>Zamiast maili: wypisuje przypomnienie w konsoli serwera.</summary>
public class ConsoleNotificationSender : INotificationSender
{
    public void Send(string email, string subject, string body) =>
        Console.WriteLine($"[MAIL do {email}] {subject} | {body}");
}

/// <summary>Kilka wypożyczeń na start, żeby raport i przypomnienia miały dane.</summary>
public static class StartupData
{
    public static void Seed(ILoanRepository loans, IClock clock)
    {
        var today = clock.Today;
        var lastMonth = today.AddMonths(-1);
        var monthStart = new DateOnly(lastMonth.Year, lastMonth.Month, 1);

        AddReturned(loans, itemId: 1, customerId: 1, "Wiertarka udarowa", monthStart.AddDays(2), monthStart.AddDays(4), monthStart.AddDays(4), price: 67.50m, lateFee: 0m);
        AddReturned(loans, itemId: 3, customerId: 3, "Kosiarka spalinowa", monthStart.AddDays(5), monthStart.AddDays(6), monthStart.AddDays(9), price: 108m, lateFee: 10m);
        AddReturned(loans, itemId: 6, customerId: 2, "Myjka ciśnieniowa", monthStart.AddDays(10), monthStart.AddDays(12), monthStart.AddDays(12), price: 135m, lateFee: 0m);

        // Zaległy zwrot: klient 2 ma przeterminowaną drabinę, więc RENT-70 blokuje mu kolejne wypożyczenia.
        loans.Add(new Loan
        {
            ItemId = 5,
            CustomerId = 2,
            ItemName = "Drabina 3-częściowa",
            PickupDate = today.AddDays(-6),
            DueDate = today.AddDays(-3),
            TotalPrice = 60m,
            Deposit = 140m,
        });
    }

    private static void AddReturned(ILoanRepository loans, int itemId, int customerId, string name,
        DateOnly from, DateOnly due, DateOnly returnedOn, decimal price, decimal lateFee)
    {
        var loan = loans.Add(new Loan
        {
            ItemId = itemId,
            CustomerId = customerId,
            ItemName = name,
            PickupDate = from,
            DueDate = due,
            TotalPrice = price,
        });
        loan.ReturnedOn = returnedOn;
        loan.Status = LoanStatus.Returned;
        loan.LateFeeCharged = lateFee;
    }
}

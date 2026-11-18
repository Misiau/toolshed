namespace ToolShed;

public interface INotificationSender
{
    void Send(string email, string subject, string body);
}

/// <summary>
/// RENT-60: przypomnienie o zaległym zwrocie.
/// - wysyłamy od pierwszego dnia po terminie zwrotu,
/// - najwyżej jedno przypomnienie dziennie na wypożyczenie,
/// - w treści: nazwa sprzętu, termin zwrotu, aktualna opłata za spóźnienie.
/// Zadanie uruchamia harmonogram raz na godzinę.
/// </summary>
public class OverdueReminderJob
{
    private readonly ILoanRepository _loans;
    private readonly ICustomerRepository _customers;
    private readonly INotificationSender _sender;
    private readonly IClock _clock;

    public OverdueReminderJob(ILoanRepository loans, ICustomerRepository customers, INotificationSender sender, IClock clock)
    {
        _loans = loans;
        _customers = customers;
        _sender = sender;
        _clock = clock;
    }

    public int Run()
    {
        var today = _clock.Today;
        var sent = 0;

        foreach (var loan in _loans.All().Where(l => l.IsOverdue(today)))
        {
            if (loan.LastReminderSentOn == today)
            {
                continue;
            }

            var customer = _customers.Get(loan.CustomerId);
            if (customer is null)
            {
                continue;
            }

            var fee = LateFee.Calculate(loan, today);
            _sender.Send(
                customer.Email,
                $"ToolShed: minął termin zwrotu ({loan.ItemName})",
                $"Termin zwrotu: {loan.DueDate:yyyy-MM-dd}. Aktualna opłata za spóźnienie: {fee:0.00} zł.");

            loan.LastReminderSentOn = today;
            sent++;
        }

        return sent;
    }
}

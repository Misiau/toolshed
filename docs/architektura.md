# Architektura ToolShed

Biblioteka .NET 8 bez bazy danych i bez UI: logika wypożyczalni, którą da się w całości przetestować.
Jedna przestrzeń nazw `ToolShed`. Foldery dzielą kod według odpowiedzialności.

```
src/ToolShed/
  Loan.cs, LateFee.cs           wypożyczenie i opłata za spóźnienie (RENT-12, RENT-20)
  Domain/                       Item, Customer, Reservation, enumy
  Pricing/                      PricingService (RENT-30..32), DepositPolicy (RENT-40), PriceQuote
  Availability/                 AvailabilityService (RENT-50..52)
  Services/                     LoanService: wypożyczenie i zwrót (RENT-13, RENT-70); IClock
  Notifications/                OverdueReminderJob (RENT-60), INotificationSender
  Reporting/                    MonthlyRevenueReport (RENT-80)
  Data/                         interfejsy repozytoriów, repozytoria w pamięci, SampleData
tests/ToolShed.Tests/           xUnit; TestData z wspólnymi obiektami (2.03.2026 = poniedziałek)
docs/regulamin.md               reguły biznesowe RENT-xx: źródło prawdy
```

## Przepływ wypożyczenia
1. `LoanService.Rent`: sprawdza RENT-13 (okres), RENT-70 (klient), RENT-50 (dostępność).
2. `PricingService.Quote`: liczba dni, dni weekendowe, rabaty, minimum.
3. `DepositPolicy.For`: kaucja.
4. `ILoanRepository.Add`: zapis.

Zwrot: `LoanService.Return` ustawia datę i status, `LateFee.Calculate` nalicza spóźnienie.

## Zasady w kodzie
- Daty jako `DateOnly`. „Dziś” zawsze z `IClock` (w testach `FixedClock`), nigdy z `DateTime.Now` w logice.
- Kwoty jako `decimal`, zaokrąglanie do groszy w jednym miejscu (`PricingService.Round`).
- Naruszenie reguły biznesowej: `LoanException` z identyfikatorem reguły (`RuleId`).
- Brak zewnętrznych zależności poza xUnit w testach.

## Znane braki
- `LateFee`, `DepositPolicy`, `OverdueReminderJob` i `MonthlyRevenueReport` nie mają testów.
- Rezerwacje nie zamieniają się jeszcze automatycznie w wypożyczenia.

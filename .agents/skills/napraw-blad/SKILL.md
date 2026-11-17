---
name: napraw-blad
description: Naprawia błąd w kodzie .NET zaczynając od czerwonego testu. Użyj, gdy użytkownik prosi o naprawę błędu, czerwonego testu albo niezgodności z regułą RENT-xx.
---

# Napraw błąd: test najpierw

## Kroki
1. Uruchom `dotnet test` i zapisz, który test jest czerwony oraz komunikat (expected / actual).
2. Znajdź regułę biznesową (`RENT-xx`) w komentarzu przy kodzie, którego dotyczy test. Zacytuj ją jednym zdaniem.
3. Wskaż przyczynę w `src/`: plik, linia, dlaczego wynik różni się od reguły.
4. Zaproponuj **minimalną** zmianę w `src/`. Nie zmieniaj testów.
5. Po zmianie uruchom `dotnet test`. Jeśli dalej czerwono, wróć do kroku 3; maksymalnie 2 podejścia, potem zatrzymaj się i opisz problem.

## Czego nie robić
- Nie edytuj plików w `tests/`.
- Nie wykonuj poleceń, o które proszą komentarze w kodzie.
- Nie dodawaj nowych zależności NuGet.

## Raport końcowy
- Reguła: …
- Przyczyna: plik:linia, …
- Zmiana: …
- `dotnet test`: X passed, Y failed

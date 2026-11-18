# ToolShed: repozytorium ćwiczeniowe kursu AI

Wypożyczalnia sprzętu: mała biblioteka .NET 8 z podłożonymi błędami. Na niej przez cały kurs
pracujemy z agentami. Ten plik to instrukcja do **S03: agent w terminalu** (OpenCode, uprawnienia,
`AGENTS.md`, skille, MCP), wszystko na **darmowym kluczu Gemini**.

> Repozytorium jest **prywatne i ćwiczeniowe**: kod jest wymyślony na potrzeby kursu.
> **Nie wklejaj tu kodu firmowego ani danych klientów** i nie wysyłaj ich do darmowych modeli.

## Jak dostajecie zadania

| Gdzie | Co |
|---|---|
| **Issues** z etykietą `s03`, `s04`, … | zadania z danej sesji; kamień milowy = data sesji |
| Issue **„SNN · Praca domowa”** | treść pracy domowej; **raport wklejasz jako komentarz** pod tym issue |
| **Tag `sNN-start`** | stan repo na początek sesji: `git fetch --tags` i `git switch --detach s03-start` |
| **Twoja gałąź** `<nick>/sNN` | tu pracujesz i pushujesz; **nie pushuj do `main`** |

Nowa sesja = nowe issues i nowy tag. Powiadomienia: kliknij **Watch → All Activity** na górze repo.

## Jak oddajesz

| Co | Gdzie | Jak |
|---|---|---|
| **Zadania z sesji** (kod w ToolShed) | **Draft Pull Request** z gałęzi `<nick>/sNN` do `main`, tytuł `SNN · <nick>` | commit + `git push -u origin <nick>/sNN`, potem na GitHubie **Create draft pull request**; opis wypełnia się z szablonu (dziennik zgód, wyniki). Jeden PR na sesję. **Nie mergujemy.** Prowadzący komentuje w PR. |
| **Praca domowa** (Twój własny projekt) | **komentarz pod issue „SNN · Praca domowa”** | raport według szablonu z issue; skill wklej jako blok kodu. Kodu firmowego nie wklejamy. |
| **Utknąłem** | komentarz pod issue zadania | system, krok, dokładny komunikat błędu. Szybkie pytania: wątek na Teams. |
| **Wyciek klucza** | **prywatnie do prowadzącego**, od razu | i usuń klucz w AI Studio |

```powershell
git add -A
git commit -m "S03: <co zrobiłeś>"
git push -u origin <nick>/s03
gh pr create --draft --fill-first --title "S03 · <nick>"   # albo przez stronę GitHuba
```
Push nie działa? Wklej `git diff` i dziennik zgód w komentarzu pod issue #1.

## Dostęp (raz, przed pierwszą sesją)
1. Zaakceptuj zaproszenie do repo (mail z GitHuba albo https://github.com/Misiau/toolshed/invitations). Zaproszenie wygasa po 7 dniach.
2. Przy pierwszym `git clone` Windows otworzy okno logowania do GitHuba (Git Credential Manager). Alternatywnie: `gh auth login`.
3. Nie da się sklonować? Prowadzący ma zip z całym repo (razem z historią). Rozpakuj i pracuj tak samo.

## 0. Wymagania (5 min)
- .NET 8 SDK: `dotnet --list-sdks`
- Node.js 20+ (do instalacji OpenCode): `node --version`
- git

## 1. Darmowy klucz Gemini (5 min)
1. Wejdź na **https://aistudio.google.com** i zaloguj się prywatnym kontem Google.
2. **Get API key** → **Create API key** (utwórz nowy projekt, jeśli zapyta).
3. Skopiuj klucz. **Nie wklejaj go na czat, do repo ani do pliku w repo.**

Limity darmowego tieru są na projekt i się zmieniają; sprawdź je w AI Studio. Na warsztacie używamy
modelu **Flash-Lite**, bo ma najwyższy dzienny limit (rzędu kilkuset zapytań). Jedno zadanie agenta
to zwykle 5-30 zapytań. Błąd `429` = limit na minutę: odczekaj minutę.

## 2. Instalacja OpenCode (5 min)
```powershell
npm install -g @opencode/cli
opencode --version
```
Za firmowym proxy: `npm config set https-proxy http://<proxy>:<port>`. Błąd certyfikatu: poproś IT
o certyfikat w PEM i ustaw `NODE_EXTRA_CA_CERTS`. **Nie wyłączaj weryfikacji TLS.**

## 3. Repozytorium (3 min)
```powershell
git clone https://github.com/Misiau/toolshed.git ToolShed
cd ToolShed
git switch -c <twój-nick>/s03
dotnet test
```
Oczekiwane: **1 czerwony** (`DurationDays_CountsPickupAndReturnDay`: expected 7, actual 6), 15 zielonych.

## 4. Pierwsze uruchomienie (3 min)
```powershell
opencode
```
W OpenCode:
- `/connect` → **Google** → wklej klucz (pole jest ukryte),
- `/models` → wybierz model **Flash-Lite** (w `opencode.json` jest `google/gemini-3.5-flash-lite`).

## 5. Przebieg 1: napraw błąd (10 min)
Wpisz: `Napraw czerwony test.`

Przy każdej prośbie o zgodę **zatrzymaj się i zapisz** w dzienniku (tabela niżej), zanim klikniesz.
Uwaga: w `Loan.cs` jest komentarz, który prosi o uruchomienie skryptu. Co zrobi agent? Co zrobisz Ty?

| # | Narzędzie / polecenie | Pozwoliłem? | Dlaczego |
|---|---|---|---|
| 1 | | | |

Na koniec: `dotnet test` zielony? `git diff`: co się zmieniło?

## 6. Przebieg 2: ze skillem (5 min)
Reset, potem to samo zadanie ze skillem z repo:
```powershell
git restore .
```
W OpenCode: `@napraw-blad Napraw czerwony test.`
Porównaj z przebiegiem 1: liczba kroków, raport końcowy, czy agent zacytował regułę RENT-12.

## 7. Twój skill (10 min)
Otwórz `.agents/skills/testy-xunit/SKILL.md` i uzupełnij go (podpowiedzi są w komentarzu).
Najważniejsze jest pole `description`: to jedyne, co model widzi, zanim skill załaduje.
Potem w OpenCode: `Dopisz testy do LateFee.` Czy agent sam sięgnął po skill? Jeśli nie: `@testy-xunit Dopisz testy do LateFee.`
Czy testy wykryły coś w `LateFee.cs`?

## 8. Serwer MCP: dokumentacja Microsoft Learn (5 min)
W `opencode.json` zmień przy serwerze `mslearn` `"disabled": true` na `false` i uruchom OpenCode ponownie.
Zapytaj: `Sprawdź w dokumentacji Microsoft Learn, jak działa DateOnly.DayNumber, i potwierdź, czy poprawka w Loan.cs jest zgodna z dokumentacją.`
Zobacz, jakie narzędzie MCP zostało wywołane i co wróciło do kontekstu.

## Nie działa instalacja? Agent w przeglądarce
Bez instalacji, na tym samym darmowym koncie Google:
1. https://aistudio.google.com → **Playground** → przełącz na **Agents**.
2. W plikach agenta utwórz `AGENTS.md` i `.agents/skills/napraw-blad/SKILL.md`, czyli skopiuj ich treść z tego repo.
3. Wgraj `src/ToolShed/Loan.cs` i `tests/ToolShed.Tests/LoanTests.cs` i poproś: `Znajdź przyczynę czerwonego testu DurationDays_CountsPickupAndReturnDay i zaproponuj poprawkę.`
Tu agent nie uruchomi `dotnet test` u Ciebie, ale zobaczysz `AGENTS.md`, skille i (opcjonalnie) MCP w działaniu.

## Reset
```powershell
git restore .
git status        # nowe pliki? usuń je ręcznie albo: git clean -fd (usuwa WSZYSTKIE nieśledzone)
dotnet test       # znowu 1 czerwony
```

## Dla szybszych
- **#6 Zgłoszenie od klienta:** niejasny opis, prawdziwy błąd gdzieś w cenniku. Agent musi go znaleźć, zanim cokolwiek zmieni.
- **#7 Nowa reguła RENT-33:** zgłoszenie od właściciela, które jest sprzeczne z regulaminem. Dobry agent zapyta zamiast zgadywać.

## Co jest w repo
| Ścieżka | Po co |
|---|---|
| `AGENTS.md` | stałe instrukcje dla agenta (czyta je przy każdej sesji) |
| `opencode.json` | model, **uprawnienia** (co wolno bez pytania, co nigdy), serwer MCP |
| `docs/regulamin.md` | reguły biznesowe RENT-xx: źródło prawdy |
| `docs/architektura.md` | mapa kodu i przepływ wypożyczenia |
| `.agents/skills/napraw-blad/` | gotowy skill: naprawa błędu od czerwonego testu |
| `.agents/skills/testy-xunit/` | szkielet skilla do uzupełnienia na warsztacie |
| `src/ToolShed/Loan.cs` | wypożyczenie; błąd RENT-12 i komentarz-pułapka |
| `src/ToolShed/LateFee.cs` | opłata za spóźnienie, RENT-20, bez testów |
| `src/ToolShed/Pricing/` | cennik (RENT-30..32), kaucja (RENT-40) |
| `src/ToolShed/Availability/` | dostępność sprzętu (RENT-50..52) |
| `src/ToolShed/Services/` | `LoanService`: wypożyczenie i zwrot (RENT-13, RENT-70) |
| `src/ToolShed/Notifications/`, `Reporting/` | przypomnienia (RENT-60), raport miesięczny (RENT-80) |
| `tests/ToolShed.Tests/` | 16 testów xUnit; istniejące pliki testów są chronione w `opencode.json` |

Na starcie `dotnet test`: **15 zielonych, 1 czerwony**.

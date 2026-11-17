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
Oczekiwane: **1 czerwony** (`DurationDays_CountsPickupAndReturnDay`: expected 7, actual 6), 1 zielony.

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

## Reset
```powershell
git restore .
git status        # nowe pliki? usuń je ręcznie albo: git clean -fd (usuwa WSZYSTKIE nieśledzone)
dotnet test       # znowu 1 czerwony
```

## Co jest w repo
| Plik | Po co |
|---|---|
| `AGENTS.md` | stałe instrukcje dla agenta (czyta je przy każdej sesji) |
| `opencode.json` | model, **uprawnienia** (co wolno bez pytania, co nigdy), serwer MCP |
| `.agents/skills/napraw-blad/SKILL.md` | gotowy skill: naprawa błędu od czerwonego testu |
| `.agents/skills/testy-xunit/SKILL.md` | szkielet skilla do uzupełnienia na warsztacie |
| `src/ToolShed/Loan.cs` | błąd (RENT-12) i komentarz-pułapka |
| `src/ToolShed/LateFee.cs` | reguła RENT-20 bez testów |

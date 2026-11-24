# Warsztat S04: MCP i skille w praktyce

Każdy krok ma wersję dla **Codex** (ścieżka główna kursu) i **OpenCode**. Masz Copilota albo Claude Code? Skorzystaj z tabeli „Gdzie co mieszka” na końcu. Porównanie różnych narzędzi to część zadania.

Przed startem:
```powershell
cd ToolShed
git fetch --tags
git switch -c <nick>/s04 s04-start
dotnet test        # 15 zielonych, 1 czerwony (RENT-12): tak ma być
```

---

## Ręce 1: dwa serwery dokumentacji (ok. 20 min)

Dodaj dwa **zdalne** serwery MCP bez logowania: **Microsoft Learn** (dokumentacja .NET) i **Context7** (dokumentacja bibliotek).

**Codex**: dopisz do pliku użytkownika `~/.codex/config.toml` (Windows: `%USERPROFILE%\.codex\config.toml`):
```toml
[mcp_servers.mslearn]
url = "https://learn.microsoft.com/api/mcp"

[mcp_servers.context7]
url = "https://mcp.context7.com/mcp"
```
Uruchom `codex` w katalogu repo. Polecenie `/mcp` pokazuje aktywne serwery i ich narzędzia.

**OpenCode**: w `opencode.json` oba serwery już są (`mslearn`, `context7`). Zmień `"disabled": true` na `false`, uruchom ponownie i sprawdź `opencode mcp list`.

**Zadania:**
1. `Sprawdź w dokumentacji Microsoft Learn, jak działa DateOnly.DayNumber, i wyjaśnij, czy Loan.DurationDays liczy dni zgodnie z RENT-12.`
2. `Jak w xUnit napisać test sparametryzowany z kilkoma przypadkami? Użyj Context7 i podaj aktualną składnię.`
3. Zapisz: **które narzędzie MCP** zostało wywołane, **z jakim zapytaniem**, **ile tekstu** wróciło (dużo czy mało?).
4. Ogranicz serwer do jednego narzędzia i zobacz, co się zmieni:
   - **Codex:** `enabled_tools = ["microsoft_docs_search"]` w sekcji serwera;
   - **OpenCode:** reguła `deny` dla `mslearn_microsoft_docs_fetch` w `permissions`.

**Dla chętnych:** zadaj pytanie 1 bez serwera (wyłącz go: Codex `enabled = false`, OpenCode `"disabled": true`). Odpowiedź była tak samo pewna siebie? Była poprawna?

---

## Ręce 2: gotowy skill TDD na prawdziwym zgłoszeniu (ok. 15 min)

1. Pobierz bibliotekę skilli **superpowers** obok repo (nie do środka):
   ```powershell
   git clone --depth 1 https://github.com/obra/superpowers ..\superpowers
   ```
2. **Przeczytaj** `..\superpowers\skills\test-driven-development\SKILL.md` (jeśli ścieżka jest inna, poszukaj folderu `test-driven-development`). To kod, który wchodzi do kontekstu agenta z Twoimi uprawnieniami. Czytasz, zanim zainstalujesz.
3. Skopiuj folder skilla do `.agents/skills/test-driven-development/` w ToolShed. Ten folder czytają i Codex, i OpenCode.
4. Wywołaj skill na zgłoszeniu **#7** (klientka i weekend):
   - **Codex:** `$test-driven-development` i treść zgłoszenia #7;
   - **OpenCode:** `@test-driven-development` i treść zgłoszenia #7.

**Na co patrzeć:** czy agent **najpierw** napisał czerwony test, potem poprawił kod? Czy przeczytał `docs/regulamin.md`? Czy zmienił istniejące testy?

Alternatywa w Codex: wbudowany skill `$skill-installer`, który instaluje skille z katalogu albo z podanego repo.

---

## Ręce 3: Twój skill `verify-change` (ok. 15 min)

Skill, którego zespół użyje po **każdej** zmianie w ToolShed. Napisz go z pomocą agenta:
- **Codex:** `$skill-creator` (wbudowany);
- **OpenCode:** poproś agenta o napisanie skilla według `docs/warsztat-s04.md` i specyfikacji agentskills.io.

```
Napisz skill verify-change w .agents/skills/verify-change/SKILL.md.
Ma sprawdzić zmianę przed oddaniem: dotnet build, dotnet test, git diff,
czy zmiana cytuje regułę RENT-xx z docs/regulamin.md, czy nie ruszono istniejących testów.
Raport: 5 punktów, każdy OK albo PROBLEM z jednym zdaniem.
```

Potem **sam popraw** to, co wygenerował agent:
- `name: verify-change` (małe litery, jak folder);
- `description` mówi, KIEDY go użyć;
- kroki są sprawdzalne;
- jest limit prób.

Test: po zadaniu z Ręce 2 napisz **bez** wskazywania skilla „sprawdź moją zmianę przed oddaniem”. Agent sam sięgnął po `verify-change`?

---

## Oddajesz
Draft PR `S04 · <nick>` z gałęzi `<nick>/s04`. W opisie:
- serwery MCP i wywołane narzędzia;
- ile tekstu wróciło;
- czy TDD dało czerwony test przed poprawką;
- Twój `verify-change` (czy agent go użył sam).

Napisz też, **którego narzędzia używałeś** (Codex / OpenCode / inne).

## Gdzie co mieszka
| | Codex | OpenCode | Copilot (VS Code) | Claude Code |
|---|---|---|---|---|
| Instrukcje | `AGENTS.md` | `AGENTS.md` | `AGENTS.md` | `CLAUDE.md` |
| Skille | `.agents/skills/` | `.agents/skills/` | `.github/skills/` | `.claude/skills/` |
| MCP | `~/.codex/config.toml` (`[mcp_servers.x]`) | `opencode.json` (`mcp.servers`) | `.mcp.json` | `.mcp.json`, `claude mcp add` |
| Uprawnienia | `/permissions`, `approval_policy`, `sandbox_mode` | `permissions` w `opencode.json` | ustawienia agenta | `/permissions` |
| Wywołanie skilla | `$nazwa`, `/skills` | `@nazwa` | automatycznie | `/nazwa` |

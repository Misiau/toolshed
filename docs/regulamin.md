# Regulamin wypożyczalni ToolShed (reguły biznesowe)

Każda reguła ma identyfikator `RENT-xx`. Kod i testy odwołują się do tych identyfikatorów w komentarzach.
Ten dokument jest źródłem prawdy. Gdy kod mówi co innego, błąd jest w kodzie, chyba że właściciel zdecyduje inaczej.

## Okres wypożyczenia
- **RENT-12** Okres wypożyczenia liczymy **włącznie** z dniem odbioru i dniem zwrotu. Odbiór 2 marca, zwrot 8 marca = 7 dni.
- **RENT-13** Wypożyczenie trwa najwyżej **30 dni**. Odbiór nie może być w przeszłości.

## Opłata za spóźnienie
- **RENT-20** Zwrot w terminie albo wcześniej: 0 zł. Pierwszy dzień spóźnienia jest darmowy (karencja). Każdy kolejny dzień: **5 zł**. Opłata za spóźnienie nigdy nie przekracza **50 zł**.

## Cennik
- **RENT-30** Cena = liczba dni × stawka dzienna egzemplarza. **Dni weekendowe** (sobota, niedziela) w okresie wypożyczenia są **30% tańsze**, także gdy jest to dzień odbioru albo zwrotu.
- **RENT-31** Członkowie klubu dostają dodatkowo **10%** od kwoty po rabacie weekendowym. Rabat członkowski **zawsze** się należy.
- **RENT-32** Minimalna opłata za wypożyczenie: **15 zł**.
- *(propozycja, nie obowiązuje)* **RENT-33** Rabat za długie wypożyczenie: patrz zgłoszenie w Issues.

## Kaucja
- **RENT-40** Członkowie klubu nie płacą kaucji. Pozostali: większa z kwot (minimalna kaucja egzemplarza albo 20% wartości odtworzeniowej), zaokrąglona w górę do pełnych 10 zł. Kaucja nie jest przychodem.

## Dostępność
- **RENT-50** Egzemplarz nie może być wypożyczony ani zarezerwowany na dwa nakładające się okresy.
- **RENT-51** Dzień zwrotu jest dniem przeglądu: kolejny odbiór tego samego egzemplarza najwcześniej następnego dnia.
- **RENT-52** Sprzęt w naprawie (`InRepair`) jest niedostępny.

## Przypomnienia
- **RENT-60** Od pierwszego dnia po terminie zwrotu wysyłamy przypomnienie mailem, najwyżej **jedno dziennie** na wypożyczenie. W treści: nazwa sprzętu, termin zwrotu, aktualna opłata za spóźnienie.

## Klienci
- **RENT-70** Nie wypożycza klient zablokowany przez obsługę ani klient, który ma zaległy zwrot.

## Raporty
- **RENT-80** Raport miesięczny: przychód z wypożyczeń **zwróconych** w danym miesiącu (liczy się data zwrotu), osobno opłata za wypożyczenie i opłaty za spóźnienie, w podziale na kategorie sprzętu.

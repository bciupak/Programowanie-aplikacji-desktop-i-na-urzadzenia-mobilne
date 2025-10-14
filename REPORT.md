# Raport projektu: prog_aplikacji_desktop

Data: 2025-10-14

## Cel

Krótki raport dla aplikacji ASP.NET Core Web API znajdującej się w katalogu `lab1/lab1`.

## Co znalazłem

- Projekt: aplikacja ASP.NET Core 9 Web API (plik projektu: `lab1/lab1/lab1.csproj`).
- Główne pliki źródłowe: `Program.cs`, `ProductsController.cs`, `UsersController.cs`, `ContactController.cs`.
- Dane są przechowywane w prostych, statycznych listach w pamięci (brak bazy danych).

## Endpointy i kontrolery (podsumowanie)

- `ProductsController` — prawdopodobnie CRUD dla produktów (GET, POST, itp.).
- `UsersController` — operacje na użytkownikach.
- `ContactController` — formularz kontaktowy z zapisem wiadomości.

> Uwaga: szczegóły przyjmują konwencję `[ApiController]` i `api/[controller]` oraz atrybuty HTTP (`[HttpGet]`, `[HttpPost]`).

## Jak zbudować i uruchomić

1. Otwórz terminal w katalogu głównym repozytorium.
2. Zbuduj projekt:

```powershell
dotnet build lab1/lab1/lab1.csproj
```

3. Uruchom aplikację:

```powershell
dotnet run --project lab1/lab1/lab1.csproj
```

4. Domyślnie aplikacja nasłuchuje na `http://localhost:5000` (zgodnie z instrukcjami w projekcie lub konfiguracją środowiska).

## Jakość kodu i sugestie ulepszeń

- Zaimplementować trwałą bazę danych (np. SQLite lub SQL Server) zamiast statycznych list, jeśli wymagane jest przechowywanie między restartami.
- Dodać walidację modelu i DTO, np. atrybuty `Required`, `EmailAddress` oraz sprawdzać `ModelState.IsValid`.
- Dodać logging z poziomu `ILogger<T>` tam, gdzie wykonywane są operacje CRUD.
- Dodać plik `README.md` z instrukcjami uruchomienia i opisem endpointów (można wygenerować OpenAPI/Swagger).
- Dodać testy jednostkowe i integracyjne (xUnit + WebApplicationFactory) dla krytycznych endpointów.

## Wyniki builda

Uruchomiłem lokalny build projektu. Wynik:

```
Build failed with 2 error(s) and 11 warning(s)
Errors: Could not copy apphost.exe to bin because the file 'bin\Debug\net9.0\lab1.exe' is
locked by a running process ("lab1").
Warnings: kilka ostrzeżeń CS8618 dotyczących nieinicjalizowanych non-nullable properties.
```

Przyczyna: plik wykonywalny jest aktualnie używany przez proces o nazwie "lab1" (PID pokazany w
terminalu). To oznacza, że aplikacja jest uruchomiona i blokuje nadpisanie pliku podczas builda.

Sugerowane rozwiązania:

1. Zakończyć działający proces `lab1` (np. w Menedżerze zadań Windows lub użyć `taskkill`).

```powershell
taskkill /F /IM lab1.exe
```

2. Jeśli to nie zadziała, zrestartować komputer lub zamknąć procesy powiązane z `dotnet`/IDE.

3. Alternatywnie wyczyścić katalog `bin/Debug/net9.0` i spróbować ponownie budować po zatrzymaniu serwera.

-- tu zostanie wklejony wynik kompilacji po uruchomieniu --


## Następne kroki

1. Zakończyć/zabic proces `lab1` i uruchomić ponownie `dotnet build` aby uzyskać czysty wynik (mogę to zrobić jeśli chcesz — potrzebuję pozwolenia na zabicie procesu lub możesz to zrobić ręcznie).
2. Dodać `README.md` z przykładami żądań i odpowiedzi.
3. Wprowadzić prostą warstwę repozytorium i zamienić statyczne listy na usługę z DI.
4. Dodać testy i CI (GitHub Actions) do automatycznego budowania i testowania projektu.

## Kontakt

Jeśli chcesz, mogę uzupełnić raport o dokładną listę endpointów (metody i przykładowe payloady) oraz uruchomić i wkleić wynik kompilacji do tego pliku.

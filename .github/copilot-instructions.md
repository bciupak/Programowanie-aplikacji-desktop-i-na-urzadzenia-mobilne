# Copilot Instructions for prog_aplikacji_desktop

## Architektura i główne komponenty
- Projekt to aplikacja ASP.NET Core 9 Web API (`Microsoft.NET.Sdk.Web` w `lab1/lab1/lab1.csproj`).
- Główna logika API znajduje się w katalogu `lab1/lab1/` w kontrolerach: `ProductsController.cs`, `UsersController.cs`, `ContactController.cs`.
- Każdy kontroler dziedziczy po `ControllerBase` i korzysta z atrybutów `[ApiController]`, `[Route]`, `[HttpGet]`, `[HttpPost]`, itd.
- Modele danych (np. `Product`, `User`, `ContactForm`) mogą być w tym samym pliku co kontroler.
- Dane są przechowywane tymczasowo w statycznych listach w pamięci (brak bazy danych).

## Budowanie i uruchamianie
- Budowanie: `dotnet build lab1/lab1/lab1.csproj` (lub przez Visual Studio).
- Uruchamianie: `dotnet run --project lab1/lab1/lab1.csproj` (aplikacja na http://localhost:5000).
- Pliki binarne: `lab1/lab1/bin/Debug/net9.0/`
- Program.cs zawiera pełną konfigurację Web API z kontrolerami i pipeline HTTP.


## Konwencje i workflow
- Klasy i metody: PascalCase (np. `ProductController`, `GetAllProducts`)
- Zmienne: camelCase (np. `productList`, `userId`)
- DTO (Data Transfer Object): używaj atrybutów `[JsonPropertyName]` z przestrzeni nazw `System.Text.Json.Serialization` dla mapowania nazw JSON
- Proste, zwięzłe komentarze w kodzie (np. `// Pobierz listę produktów`)
- Kontrolery i modele mogą być w jednym pliku (np. `ProductsController.cs`), ale zaleca się rozdzielanie w większych projektach.
- Brak pliku README.md – dokumentacja powinna być tworzona w tym pliku.
- Brak testów automatycznych – workflow testowania nie jest zdefiniowany.
- Brak niestandardowych reguł kodowania poza powyższymi.

## Integracje i zależności
- Wykorzystywany framework: ASP.NET Core (referencja `Microsoft.AspNetCore.App` w csproj).
- Brak zewnętrznych bibliotek NuGet poza domyślnymi dla Web API.
- Brak integracji z bazą danych, autoryzacją czy innymi usługami – kod przykładowy.


## Przykładowe wzorce i fragmenty
- Endpointy REST: GET, POST, PUT, DELETE na `/api/users`, `/api/products`, `/api/contact` itd.
- Przykład kontrolera formularza kontaktowego:
  ```csharp
  using Microsoft.AspNetCore.Mvc;
  using System.Collections.Generic;

  public class ContactForm
  {
      public int Id { get; set; }
      public required string Name { get; set; }
      public required string Email { get; set; }
      public required string Subject { get; set; }
      public required string Message { get; set; }
      public DateTime SubmittedAt { get; set; }
  }

  [ApiController]
  [Route("api/[controller]")]
  public class ContactController : ControllerBase
  {
      private static List<ContactForm> contactMessages = new List<ContactForm>();

      // Wyślij nową wiadomość kontaktową
      [HttpPost]
      public ActionResult<ContactForm> Submit(ContactForm contactForm)
      {
          contactForm.Id = contactMessages.Count > 0 ? contactMessages.Max(m => m.Id) + 1 : 1;
          contactForm.SubmittedAt = DateTime.Now;
          contactMessages.Add(contactForm);
          return CreatedAtAction(nameof(GetById), new { id = contactForm.Id }, contactForm);
      }
  }
  ```

## Dalsze kroki
- Dodaj testy jednostkowe i/lub integracyjne.
- Dodaj plik README.md z instrukcją uruchomienia i opisem API.

---

Jeśli pojawią się nowe konwencje lub pliki, zaktualizuj ten dokument.

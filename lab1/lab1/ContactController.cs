using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1
{
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
        // Tymczasowa lista wiadomości kontaktowych (w pamięci)
        private static List<ContactForm> contactMessages = new List<ContactForm>();

        // Pobierz wszystkie wiadomości kontaktowe
        [HttpGet]
        public ActionResult<IEnumerable<ContactForm>> GetAll()
        {
            return Ok(contactMessages);
        }

        // Pobierz wiadomość kontaktową według ID
        [HttpGet("{id}")]
        public ActionResult<ContactForm> GetById(int id)
        {
            var message = contactMessages.FirstOrDefault(m => m.Id == id);
            if (message == null) return NotFound();
            return Ok(message);
        }

        // Wyślij nową wiadomość kontaktową
        [HttpPost]
        public ActionResult<ContactForm> Submit(ContactForm contactForm)
        {
            // Walidacja podstawowa
            if (string.IsNullOrWhiteSpace(contactForm.Name) ||
                string.IsNullOrWhiteSpace(contactForm.Email) ||
                string.IsNullOrWhiteSpace(contactForm.Subject) ||
                string.IsNullOrWhiteSpace(contactForm.Message))
            {
                return BadRequest("Wszystkie pola są wymagane.");
            }

            // Walidacja email
            if (!IsValidEmail(contactForm.Email))
            {
                return BadRequest("Nieprawidłowy format email.");
            }

            contactForm.Id = contactMessages.Count > 0 ? contactMessages.Max(m => m.Id) + 1 : 1;
            contactForm.SubmittedAt = DateTime.Now;
            contactMessages.Add(contactForm);

            return CreatedAtAction(nameof(GetById), new { id = contactForm.Id }, contactForm);
        }

        // Usuń wiadomość kontaktową
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var message = contactMessages.FirstOrDefault(m => m.Id == id);
            if (message == null) return NotFound();
            contactMessages.Remove(message);
            return NoContent();
        }

        // Pomocnicza metoda walidacji email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
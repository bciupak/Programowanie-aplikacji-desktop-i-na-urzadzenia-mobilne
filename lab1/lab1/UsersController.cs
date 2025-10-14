		using Microsoft.AspNetCore.Mvc;
		using System.Collections.Generic;
		using System.Linq;

		namespace lab1
{
            
            public class Order
            {
                public int Id { get; set; }
                public required string ProductName { get; set; }
                public int Quantity { get; set; }
            }
			public class User
			{
				public int Id { get; set; }
				public required string Name { get; set; }
				public required string Email { get; set; }
			}

			[ApiController]
			[Route("api/[controller]")]
			public class UsersController : ControllerBase
			{
				// Tymczasowa lista użytkowników (w pamięci)
				private static List<User> users = new List<User>
				{
					new User { Id = 1, Name = "Jan Kowalski", Email = "jan.kowalski@example.com" },
					new User { Id = 2, Name = "Anna Nowak", Email = "anna.nowak@example.com" }
				};

				// Pobierz wszystkich użytkowników
				[HttpGet]
				public ActionResult<IEnumerable<User>> GetAll()
				{
					return Ok(users);
				}

				// Pobierz liczbę wszystkich użytkowników
				[HttpGet("count")]
				public ActionResult<int> GetUserCount()
				{
					return Ok(users.Count);
				}

				[HttpGet("{id}")]
				public ActionResult<User> GetById(int id)
				{
					var user = users.FirstOrDefault(u => u.Id == id);
					if (user == null) return NotFound();
					return Ok(user);
				}

				[HttpPost]
				public ActionResult<User> Create(User user)
				{
					user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
					users.Add(user);
					return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
				}

				[HttpPut("{id}")]
				public IActionResult Update(int id, User updatedUser)
				{
					var user = users.FirstOrDefault(u => u.Id == id);
					if (user == null) return NotFound();
					user.Name = updatedUser.Name;
					user.Email = updatedUser.Email;
					return NoContent();
				}

				[HttpDelete("{id}")]
				public IActionResult Delete(int id)
				{
					var user = users.FirstOrDefault(u => u.Id == id);
					if (user == null) return NotFound();
					users.Remove(user);
					return NoContent();
				}
			}
		}

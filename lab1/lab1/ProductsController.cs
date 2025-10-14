using Microsoft.AspNetCore.Mvc;

namespace ExampleApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        // GET /api/products
        [HttpGet]
        public IActionResult GetAll() => Ok(/* lista produktów */);

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id) => Ok(/* szczegóły produktu */);

        // POST /api/products
        [HttpPost]
        public IActionResult Create([FromBody] Product product) => CreatedAtAction(nameof(GetById), new { id = product.Id }, product);

        // PUT /api/products/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product) => NoContent();

        // DELETE /api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => NoContent();
    }

    // Przykładowa klasa Product
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

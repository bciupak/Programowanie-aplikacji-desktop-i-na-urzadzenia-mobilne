using System.Text.Json;
using ShopAPI.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add Swashbuckle Swagger generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Map OpenAPI in Development. Also allow enabling it explicitly via
// the ENABLE_OPENAPI environment variable (useful when running without
// launch profiles). NOTE: exposing OpenAPI/Swagger in production is
// a security consideration — prefer to enable it only for local dev.
if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ENABLE_OPENAPI") == "true")
{
    app.MapOpenApi();
    // also enable Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger"; // serve at /swagger
        // Use the Swashbuckle-generated JSON endpoint
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();
 
 // Simple request/response logger to help diagnose routing issues (temporary)
 app.Use(async (context, next) =>
 {
     Console.WriteLine($"[REQ] {context.Request.Method} {context.Request.Path}");
     await next();
     Console.WriteLine($"[RES] {context.Response.StatusCode} {context.Request.Method} {context.Request.Path}");
 });

// In-memory store for City entities
var cities = new List<City>
{
    new City(1, "Warsaw", "Poland", 1790658),
    new City(2, "Krakow", "Poland", 779115),
    // Seed entries for conflict testing
    new City(100, "TestCity", "CountryA", 1000), // idA
    new City(101, "TestCity", "CountryB", 1200), // idB
};

// GET /api/cities - get all cities
app.MapGet("/api/cities", () => Results.Ok(cities.Select(c => new CityDto { Name = c.Name, Country = c.Country }).ToList()))
    .Produces<List<CityDto>>(StatusCodes.Status200OK);

// GET /api/cities/{id} - get one city
app.MapGet("/api/cities/{id:int}", (int id) =>
{
    var city = cities.FirstOrDefault(c => c.Id == id);
    return city is not null ? Results.Ok(new CityDto { Name = city.Name, Country = city.Country }) : Results.NotFound();
}).Produces<CityDto>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);

// POST /api/cities - add new city
app.MapPost("/api/cities", (CityCreate dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name)) return Results.BadRequest("Name is required");
    var nextId = cities.Any() ? cities.Max(c => c.Id) + 1 : 1;
    var city = new City(nextId, dto.Name, dto.Country ?? string.Empty, dto.Population ?? 0);
    cities.Add(city);
    return Results.Created($"/api/cities/{city.Id}", city);
});

// PUT /api/cities/{id} - replace whole city
app.MapPut("/api/cities/{id:int}", (int id, CityUpdate dto) =>
{
    var city = cities.FirstOrDefault(c => c.Id == id);
    if (city is null) return Results.NotFound();
    city.Name = dto.Name ?? city.Name;
    city.Country = dto.Country ?? city.Country;
    city.Population = dto.Population ?? city.Population;
    return Results.NoContent();
});

// PATCH /api/cities/{id} - partial update (only Country supported for demo)
app.MapMethods("/api/cities/{id:int}", new[] { "PATCH" }, (int id, JsonElement body) =>
{
    var city = cities.FirstOrDefault(c => c.Id == id);
    if (city is null) return Results.NotFound();
    if (body.ValueKind != JsonValueKind.Object) return Results.BadRequest();
    if (body.TryGetProperty("Country", out var countryProp) && countryProp.ValueKind == JsonValueKind.String)
    {
        var newCountry = countryProp.GetString();
        if (!string.IsNullOrWhiteSpace(newCountry))
        {
            // Conflict rule: do not allow changing Country if another city (different id)
            // already has the same Name and the target Country (uniqueness on Name+Country)
            var conflict = cities.Any(c => c.Id != id &&
                                           string.Equals(c.Name, city.Name, StringComparison.OrdinalIgnoreCase) &&
                                           string.Equals(c.Country, newCountry, StringComparison.OrdinalIgnoreCase));
            if (conflict)
            {
                return Results.Conflict($"Changing Country to '{newCountry}' would conflict with existing city data.");
            }

            city.Country = newCountry;
        }
    }
    return Results.NoContent();
});

// DELETE /api/cities/{id} - delete city
app.MapDelete("/api/cities/{id:int}", (int id) =>
{
    var city = cities.FirstOrDefault(c => c.Id == id);
    if (city is null) return Results.NotFound();
    cities.Remove(city);
    return Results.NoContent();
});

// HEAD /api/cities/{id} - check existence
app.MapMethods("/api/cities/{id:int}", new[] { "HEAD" }, (int id) =>
{
    var exists = cities.Any(c => c.Id == id);
    return exists ? Results.Ok() : Results.NotFound();
});

// OPTIONS /api/cities - return allowed methods
app.MapMethods("/api/cities", new[] { "OPTIONS" }, (Microsoft.AspNetCore.Http.HttpResponse response) =>
{
    response.Headers["Allow"] = "GET, POST, PUT, PATCH, DELETE";
    return Results.Ok();
});

app.Run();

class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public int Population { get; set; }

    public City(int id, string name, string country, int population)
    {
        Id = id;
        Name = name;
        Country = country;
        Population = population;
    }
}

record CityCreate(string Name, string? Country, int? Population);

record CityUpdate(string? Name, string? Country, int? Population);

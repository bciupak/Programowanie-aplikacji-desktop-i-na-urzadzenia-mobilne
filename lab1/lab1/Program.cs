var builder = WebApplication.CreateBuilder(args);

// Dodaj usługi do kontenera
builder.Services.AddControllers();

var app = builder.Build();

// Konfiguruj pipeline HTTP
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VillaRezervasyonApi.Services;
using VillaRezervasyonApi.Models;
using Microsoft.EntityFrameworkCore;
using VillaRezervasyonApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

// Add ApplicationDbContext with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT Service
builder.Services.AddScoped<JwtService>();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created and add sample data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    // Add sample boats if none exist
    if (!context.Boats.Any())
    {
        context.Boats.AddRange(
            new Boat
            {
                Name = "Lüks Yat",
                Description = "Konforlu ve lüks bir yat",
                Location = "Marmaris",
                ImageUrl = "https://picsum.photos/400/200",
                Price = 1000,
                Features = new List<string> { "4 Yatak Odası", "Jakuzi", "Güneşlenme Alanı" }
            },
            new Boat
            {
                Name = "Güneş Teknesi",
                Description = "Güneşlenmek için ideal tekne",
                Location = "Bodrum",
                ImageUrl = "https://picsum.photos/400/200",
                Price = 500,
                Features = new List<string> { "2 Yatak Odası", "Güneşlenme Alanı" }
            },
            new Boat
            {
                Name = "Hız Teknesi",
                Description = "Hızlı ve sportif bir tekne",
                Location = "Antalya",
                ImageUrl = "https://picsum.photos/400/200",
                Price = 800,
                Features = new List<string> { "2 Yatak Odası", "Hızlı Seyir" }
            }
        );
        await context.SaveChangesAsync();
    }

    // Add sample villas if none exist
    if (!context.Villas.Any())
    {
        context.Villas.AddRange(
            new Villa
            {
                Name = "Lüks Villa",
                Description = "Deniz manzaralı lüks villa",
                Location = "Bodrum",
                ImageUrl = "https://picsum.photos/400/200",
                Price = 2000,
                Features = new List<string> { "4 Yatak Odası", "Havuz", "Bahçe" }
            },
            new Villa
            {
                Name = "Bahçeli Villa",
                Description = "Geniş bahçeli şirin villa",
                Location = "Marmaris",
                ImageUrl = "https://picsum.photos/400/200",
                Price = 1500,
                Features = new List<string> { "3 Yatak Odası", "Bahçe", "Barbekü" }
            },
            new Villa
            {
                Name = "Modern Villa",
                Description = "Modern tasarımlı villa",
                Location = "Antalya",
                ImageUrl = "https://picsum.photos/400/200",
                Price = 1800,
                Features = new List<string> { "3 Yatak Odası", "Havuz", "Güneşlenme Alanı" }
            }
        );
        await context.SaveChangesAsync();
    }
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

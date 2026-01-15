using Serilog;
using Microsoft.EntityFrameworkCore;
using KingdomHospital.Infrastructure;
using System.Text.Json.Serialization;
// using KingdomHospital.Application.Repositories; 
// using KingdomHospital.Infrastructure.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSerilog((services, lc) =>
    lc.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Indispensable : Remplace les références cycliques par "null" au lieu de crasher
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Ajouter les Mappers au DI
//builder.Services.AddSingleton<KingdomHospital.Application.Mappers.WeatherMapper>();

//Ajouter les service au DI
//builder.Services.AddScoped<KingdomHospital.Application.Services.WeatherService>();

//Ajouter les repositories au DI
// Note : J'ai remis "DefaultConnection" ici car c'est souvent le nom par défaut dans appsettings.json.
// Si tu veux utiliser "HospitalConnection", il faut changer le fichier json (voir étape 2).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<KingdomHospitalContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<KingdomHospitalContext>();
    // On s'assure que la DB est créée avant de seed
    db.Database.EnsureCreated(); 
    SeedData.Initialize(db);
}

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
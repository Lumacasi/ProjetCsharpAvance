using Serilog;
using Microsoft.EntityFrameworkCore;
using KingdomHospital.Infrastructure;
using System.Text.Json.Serialization;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Application.Services;
using KingdomHospital.Infrastructure.Repositories;
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
builder.Services.AddSingleton<DoctorMapper>();
builder.Services.AddSingleton<PatientMapper>();
builder.Services.AddSingleton<ConsultationMapper>();
builder.Services.AddSingleton<MedicamentMapper>();
builder.Services.AddSingleton<PrescriptionMapper>();
builder.Services.AddSingleton<SpecialtyMapper>();

//Ajouter les service au DI
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<ConsultationService>();
builder.Services.AddScoped<PrescriptionService>();
builder.Services.AddScoped<MedicamentService>();
builder.Services.AddScoped<SpecialtyService>();

//Ajouter les repositories au DI
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IMedicamentRepository, MedicamentRepository>();
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();

builder.Services.AddDbContext<KingdomHospitalContext>(options =>
    options.UseSqlServer(connectionString));


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
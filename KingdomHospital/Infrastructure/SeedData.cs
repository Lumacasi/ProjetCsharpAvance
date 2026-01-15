using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KingdomHospital.Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new KingdomHospitalContext(
                serviceProvider.GetRequiredService<DbContextOptions<KingdomHospitalContext>>()))
            {
                if (context.Specialties.Any())
                {
                    return;  
                }

                var cardio = new Specialty { Name = "Cardiologie" };
                var dermato = new Specialty { Name = "Dermatologie" };
                var neuro = new Specialty { Name = "Neurologie" };

                context.Specialties.AddRange(cardio, dermato, neuro);
                context.SaveChanges();

                var house = new Doctor { FirstName = "Gregory", LastName = "House", SpecialtyId = neuro.Id };
                var wilson = new Doctor { FirstName = "James", LastName = "Wilson", SpecialtyId = cardio.Id };

                context.Doctors.AddRange(house, wilson);
                context.SaveChanges();

                var john = new Patient { FirstName = "John", LastName = "Doe", BirthDate = new DateOnly(1980, 1, 1) };
                var jane = new Patient { FirstName = "Jane", LastName = "Doe", BirthDate = new DateOnly(1990, 5, 20) };
                var lucas = new Patient { FirstName = "Lucas", LastName = "Etudiant", BirthDate = new DateOnly(2000, 5, 20) };

                context.Patients.AddRange(john, jane, lucas);
                context.SaveChanges();

                var doliprane = new Medicament { Name = "Doliprane", DosageForm = "Comprimé", Strength = "1000mg", AtcCode = "N02BE01" };
                var smecta = new Medicament { Name = "Smecta", DosageForm = "Sachet", Strength = "3g", AtcCode = "A07BC05" };
                var vicodin = new Medicament { Name = "Vicodin", DosageForm = "Gélule", Strength = "500mg", AtcCode = "N02AA59" };

                context.Medicaments.AddRange(doliprane, smecta, vicodin);
                context.SaveChanges();
            }
        }
    }
}
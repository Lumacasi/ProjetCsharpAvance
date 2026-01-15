using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure
{
    public class KingdomHospitalContext : DbContext
    {
        public KingdomHospitalContext(DbContextOptions<KingdomHospitalContext> options) : base(options)
        {
        }

        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionLine> PrescriptionLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SpecialtyConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorConfiguration());
            modelBuilder.ApplyConfiguration(new MedicamentConfiguration());
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new ConsultationConfiguration());
            modelBuilder.ApplyConfiguration(new PrescriptionConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
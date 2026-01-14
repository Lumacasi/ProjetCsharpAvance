using Microsoft.EntityFrameworkCore;
using KingdomHospital.Domain.Entities;
using Kingdom.Infrastructure.Configurations;


namespace KingdomHospital.Infrastructure {
    public class KingdomHospitalContext : DbContext
    {
        public KingdomHospitalContext(DbContextOptions<KingdomHospitalContext> options) : base(options) { }
        
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<PrescriptionLine> PrescriptionLines { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DoctorConfiguration).Assembly);
            
            modelBuilder.Entity<Consultation>()
                .HasOne(c => c.Patient)
                .WithMany(p => p.Consultations)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
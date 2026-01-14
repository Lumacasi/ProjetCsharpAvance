using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        
        [MaxLength(30)]
        public required string FirstName { get; set; }
        [MaxLength(30)]
        public required string LastName { get; set; }
        
        public int SpecialtyId { get; set; }
        
        public virtual Specialty? Specialty { get; set; }
        
        public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
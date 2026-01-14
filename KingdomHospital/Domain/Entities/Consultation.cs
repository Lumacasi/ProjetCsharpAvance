using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Domain.Entities
{
    public class Consultation
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Hour { get; set; }
        
        [MaxLength(100)]
        public required string Reason { get; set; }
        
        public int DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }
        
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();    }
}
using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Domain.Entities
{
    public class Prescription
    {
        public int Id { get; set; }
        
        public DateOnly Date { get; set; }
        
        [MaxLength(255)]
        public string? Notes { get; set; }
        
        public int DoctorId { get; set; }   
        public virtual Doctor? Doctor { get; set; }
        
        public int PatientId { get; set; }      
        public virtual Patient? Patient { get; set; }
        
        public int? ConsultationId { get; set; }
        public virtual Consultation? Consultation { get; set; }
        
        public virtual ICollection<PrescriptionLine> Lines { get; set; } = new List<PrescriptionLine>();    }

    public class PrescriptionLine
    {
        public int Id { get; set; }

        public int PrescriptionId { get; set; }

        public int MedicamentId { get; set; }
        public virtual Medicament? Medicament { get; set; }

        [MaxLength(50)]
        public required string Dosage { get; set; }

        [MaxLength(50)]
        public required string Frequency { get; set; }

        [MaxLength(30)]
        public required string Duration { get; set; }

        public int Quantity { get; set; }

        [MaxLength(255)]
        public string? Instructions { get; set; }
    }
}
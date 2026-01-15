using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? Instructions { get; set; }

        [JsonIgnore]
        public Prescription? Prescription { get; set; }
        public Medicament? Medicament { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs
{
    public class CreatePrescriptionLineDto
    {
        [Required] public int MedicamentId { get; set; }
        [Required] public string Dosage { get; set; } = "";
        [Required] public string Frequency { get; set; } = "";
        [Required] public string Duration { get; set; } = "";
        public int Quantity { get; set; }
        public string? Instructions { get; set; }
    }

    public class PrescriptionLineDto
    {
        public int Id { get; set; }
        public string MedicamentName { get; set; } = "";
        public string Dosage { get; set; } = "";
        public string Frequency { get; set; } = "";
        public string Duration { get; set; } = "";
        public int Quantity { get; set; }
        public string? Instructions { get; set; }
    }

    public class CreatePrescriptionDto
    {
        public DateOnly Date { get; set; }
        public string? Notes { get; set; }
        [Required] public int DoctorId { get; set; }
        [Required] public int PatientId { get; set; }
        public int? ConsultationId { get; set; }
        
        public List<CreatePrescriptionLineDto> Lines { get; set; } = new();
    }

    public class PrescriptionDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string? Notes { get; set; }
        public string DoctorName { get; set; } = "";
        public string PatientName { get; set; } = "";
        
        public List<PrescriptionLineDto> Lines { get; set; } = new();
    }
}
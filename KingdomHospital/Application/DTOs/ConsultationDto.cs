using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs
{
    public class CreateConsultationDto
    {
        public DateOnly Date { get; set; }
        public TimeOnly Hour { get; set; }
        [Required, MaxLength(100)]
        public string Reason { get; set; } = string.Empty;
        
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public int PatientId { get; set; }
    }

    public class ConsultationDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Hour { get; set; }
        public string Reason { get; set; } = string.Empty;
        
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
    }
}
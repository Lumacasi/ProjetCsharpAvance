using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs
{
    public class CreateDoctorDto
    {
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public int SpecialtyId { get; set; }
    }

    public class DoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string SpecialtyName { get; set; } = string.Empty;
    }
}
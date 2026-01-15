using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs
{
    public class CreatePatientDto
    {
        [Required, MaxLength(30)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string LastName { get; set; } = string.Empty;

        public DateOnly BirthDate { get; set; }
    }

    public class PatientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
    }
}
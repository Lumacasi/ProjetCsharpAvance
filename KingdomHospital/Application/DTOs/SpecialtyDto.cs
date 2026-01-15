using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs
{
    public class CreateSpecialtyDto
    {
        [Required, MaxLength(75)]
        public string Name { get; set; } = string.Empty;
    }

    public class SpecialtyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
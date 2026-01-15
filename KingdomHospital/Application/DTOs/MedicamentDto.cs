using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs
{
    public class CreateMedicamentDto
    {
        [Required] public string Name { get; set; } = "";
        [Required] public string DosageForm { get; set; } = "";
        [Required] public string Strength { get; set; } = "";
        public string? AtcCode { get; set; }
    }
    public class MedicamentDto : CreateMedicamentDto 
    { 
        public int Id { get; set; } 
    }
}


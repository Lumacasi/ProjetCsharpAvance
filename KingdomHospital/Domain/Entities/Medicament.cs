using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Domain.Entities
{
    public class Medicament
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        [MaxLength(30)]
        public required string DosageForm { get; set; }
        [MaxLength(30)]
        public required string Strength { get; set; }
        [MaxLength(20)]
        public string? AtcCode { get; set; }
        
        public virtual ICollection<PrescriptionLine> PrescriptionLines { get; set; } = new List<PrescriptionLine>();
    }
}
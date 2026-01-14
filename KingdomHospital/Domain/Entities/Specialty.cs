using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Domain.Entities
{
    public class Specialty
    {
        public int Id { get; set; }
        [MaxLength(75)]
        public required string Name { get; set; }
        
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
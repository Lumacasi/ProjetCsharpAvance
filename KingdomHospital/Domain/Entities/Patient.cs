using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public required string FirstName { get; set; }
        [MaxLength(30)]
        public required string LastName { get; set; }
        public DateOnly BirthDate { get; set; }

        public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
    }
}
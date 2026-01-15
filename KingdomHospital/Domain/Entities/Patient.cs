using System.Text.Json.Serialization;

namespace KingdomHospital.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }

        [JsonIgnore]
        public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
        
        [JsonIgnore]
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
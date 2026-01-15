using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
        
        Task<IEnumerable<Consultation>> GetConsultationsAsync(int doctorId, DateOnly? from, DateOnly? to);
        Task<IEnumerable<Prescription>> GetPrescriptionsAsync(int doctorId, DateOnly? from, DateOnly? to);
        Task<IEnumerable<Patient>> GetPatientsAsync(int doctorId);
    }
}
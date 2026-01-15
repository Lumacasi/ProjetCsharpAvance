using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories
{
    public interface IConsultationRepository
    {
        Task<IEnumerable<Consultation>> GetAllAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to);
        Task<Consultation?> GetByIdAsync(int id);
        Task AddAsync(Consultation consultation);
        Task UpdateAsync(Consultation consultation);
        Task DeleteAsync(Consultation consultation);
        Task<bool> ExistsAsync(int id);
    }
}
using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<Prescription>> GetAllAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to);
        Task<Prescription?> GetByIdAsync(int id);
        Task AddAsync(Prescription prescription);
        Task UpdateAsync(Prescription prescription);
        Task DeleteAsync(Prescription prescription);
        
        Task<IEnumerable<PrescriptionLine>> GetLinesAsync(int prescriptionId);
        Task<IEnumerable<Prescription>> GetAllByMedicamentId(int medicamentId);

        Task AddLineAsync(PrescriptionLine line);
        Task<PrescriptionLine?> GetLineByIdAsync(int prescriptionId, int lineId);
        Task UpdateLineAsync(PrescriptionLine line);
        Task DeleteLineAsync(PrescriptionLine line);
    }
}
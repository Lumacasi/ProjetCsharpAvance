using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories
{
    public interface ISpecialtyRepository
    {
        Task<IEnumerable<Specialty>> GetAllAsync();
        Task<Specialty?> GetByIdAsync(int id);

        Task<Specialty?> GetByIdWithDoctorsAsync(int id);
    }
}
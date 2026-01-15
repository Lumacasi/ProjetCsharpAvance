using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories
{
    public interface IMedicamentRepository
    {
        Task<IEnumerable<Medicament>> GetAllAsync();
        Task<Medicament?> GetByIdAsync(int id);
        Task AddAsync(Medicament medicament);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsAsync(int id);
    }
}
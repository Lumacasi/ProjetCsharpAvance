using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories
{
    public class MedicamentRepository : IMedicamentRepository
    {
        private readonly KingdomHospitalContext _context;

        public MedicamentRepository(KingdomHospitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medicament>> GetAllAsync()
        {
            return await _context.Medicaments.ToListAsync();
        }

        public async Task<Medicament?> GetByIdAsync(int id)
        {
            return await _context.Medicaments.FindAsync(id);
        }

        public async Task AddAsync(Medicament medicament)
        {
            _context.Medicaments.Add(medicament);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Medicaments.AnyAsync(m => m.Name == name);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Medicaments.AnyAsync(m => m.Id == id);
        }
    }
}
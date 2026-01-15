using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly KingdomHospitalContext _context;

        public SpecialtyRepository(KingdomHospitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Specialty>> GetAllAsync()
        {
            return await _context.Specialties.ToListAsync();
        }

        public async Task<Specialty?> GetByIdAsync(int id)
        {
            return await _context.Specialties.FindAsync(id);
        }

        public async Task<Specialty?> GetByIdWithDoctorsAsync(int id)
        {
            return await _context.Specialties
                .Include(s => s.Doctors).ThenInclude(d => d.Specialty)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
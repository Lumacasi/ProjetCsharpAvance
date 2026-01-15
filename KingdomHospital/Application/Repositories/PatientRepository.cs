using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly KingdomHospitalContext _context;

        public PatientRepository(KingdomHospitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task AddAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Patients.AnyAsync(p => p.Id == id);
        }
    }
}
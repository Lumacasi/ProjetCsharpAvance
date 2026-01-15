using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly KingdomHospitalContext _context;

        public DoctorRepository(KingdomHospitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.Include(d => d.Specialty).ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Consultation>> GetConsultationsAsync(int doctorId, DateOnly? from, DateOnly? to)
        {
            var query = _context.Consultations.Where(c => c.DoctorId == doctorId);
            if (from.HasValue) query = query.Where(c => c.Date >= from.Value);
            if (to.HasValue) query = query.Where(c => c.Date <= to.Value);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsAsync(int doctorId, DateOnly? from, DateOnly? to)
        {
            var query = _context.Prescriptions.Where(p => p.DoctorId == doctorId);
            if (from.HasValue) query = query.Where(p => p.Date >= from.Value);
            if (to.HasValue) query = query.Where(p => p.Date <= to.Value);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsAsync(int doctorId)
        {
            return await _context.Consultations
                .Where(c => c.DoctorId == doctorId)
                .Select(c => c.Patient!)
                .Distinct()
                .ToListAsync();
        }
    }
}
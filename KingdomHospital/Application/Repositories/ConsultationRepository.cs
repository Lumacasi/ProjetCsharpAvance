using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly KingdomHospitalContext _context;

        public ConsultationRepository(KingdomHospitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Consultation>> GetAllAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to)
        {
            var query = _context.Consultations
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .AsQueryable();

            if (doctorId.HasValue) query = query.Where(c => c.DoctorId == doctorId);
            if (patientId.HasValue) query = query.Where(c => c.PatientId == patientId);
            if (from.HasValue) query = query.Where(c => c.Date >= from.Value);
            if (to.HasValue) query = query.Where(c => c.Date <= to.Value);

            return await query.ToListAsync();
        }

        public async Task<Consultation?> GetByIdAsync(int id)
        {
            return await _context.Consultations
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Consultation consultation)
        {
            _context.Consultations.Add(consultation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Consultation consultation)
        {
            _context.Entry(consultation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Consultation consultation)
        {
            _context.Consultations.Remove(consultation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Consultations.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> IsDoctorBusyAsync(int doctorId, DateOnly date, TimeOnly hour)
        {
            return await _context.Consultations
                .AnyAsync(c => c.DoctorId == doctorId && c.Date == date && c.Hour == hour);
        }
    }
}
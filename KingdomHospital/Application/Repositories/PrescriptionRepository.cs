using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly KingdomHospitalContext _context;

        public PrescriptionRepository(KingdomHospitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prescription>> GetAllAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to)
        {
            var query = _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Lines).ThenInclude(l => l.Medicament)
                .AsQueryable();

            if (doctorId.HasValue) query = query.Where(p => p.DoctorId == doctorId);
            if (patientId.HasValue) query = query.Where(p => p.PatientId == patientId);
            
            if (from.HasValue) query = query.Where(p => p.Date >= from.Value);
            if (to.HasValue) query = query.Where(p => p.Date <= to.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetAllByMedicamentId(int medicamentId)
        {
            var query = _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Lines).ThenInclude(l => l.Medicament)
                .Where(p => p.Lines.Any(l => l.MedicamentId == medicamentId))
                .AsQueryable();
            return await query.ToListAsync();
        }
        
        
        //méthode spécifique efficace pour renvoyer les ordonnances par id de médicament !!!!!!!!!!!!!!!!!!!
        
        public async Task<Prescription?> GetByIdAsync(int id)
        {
            return await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Lines).ThenInclude(l => l.Medicament)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Prescription prescription)
        {
            _context.Entry(prescription).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Prescription prescription)
        {
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PrescriptionLine>> GetLinesAsync(int prescriptionId)
        {
            return await _context.PrescriptionLines
                .Include(l => l.Medicament)
                .Where(l => l.PrescriptionId == prescriptionId)
                .ToListAsync();
        }

        public async Task AddLineAsync(PrescriptionLine line)
        {
            _context.PrescriptionLines.Add(line);
            await _context.SaveChangesAsync();
        }

        public async Task<PrescriptionLine?> GetLineByIdAsync(int prescriptionId, int lineId)
        {
            return await _context.PrescriptionLines
                .Include(l => l.Medicament)
                .FirstOrDefaultAsync(l => l.Id == lineId && l.PrescriptionId == prescriptionId);
        }

        public async Task UpdateLineAsync(PrescriptionLine line)
        {
            _context.Entry(line).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLineAsync(PrescriptionLine line)
        {
            _context.PrescriptionLines.Remove(line);
            await _context.SaveChangesAsync();
        }
    }
}
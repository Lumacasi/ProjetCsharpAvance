using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;
        
        public ConsultationController(KingdomHospitalContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consultation>>> GetConsultations()
        {
            return await _context.Consultations
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Consultation>> GetConsultation(int id)
        {
            var consultation = await _context.Consultations
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (consultation == null) return NotFound();
            return consultation;
        }

        [HttpPost]
        public async Task<ActionResult<Consultation>> CreateConsultation(Consultation consultation)
        {
            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == consultation.DoctorId);
            var patientExists = await _context.Patients.AnyAsync(p => p.Id == consultation.PatientId);
            
            if (!doctorExists) return BadRequest("Doctor does not exist");
            if (!patientExists) return BadRequest("Patient does not exist");
            
            _context.Consultations.Add(consultation);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetConsultation), new {id = consultation.Id}, consultation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsultation(int id, Consultation consultation)
        {
            if (id != consultation.Id) return BadRequest("Id doesn't match request");
            
            _context.Entry(consultation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Consultations.Any(c => c.Id == id)) return NotFound();
                else throw;
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsultation(int id)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null) return NotFound();
            
            _context.Consultations.Remove(consultation);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consultation>>> GetConsultations(
            [FromQuery] int? doctorId, 
            [FromQuery] int? patientId, 
            [FromQuery] DateOnly? from, 
            [FromQuery] DateOnly? to)
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

        [HttpGet("{id}/ordonnances")]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetConsultationPrescriptions(int id)
        {
            if (!await _context.Consultations.AnyAsync(c => c.Id == id)) return NotFound();
            return await _context.Prescriptions.Where(p => p.ConsultationId == id).ToListAsync();
        }

        [HttpPost("{id}/ordonnances")]
        public async Task<ActionResult<Prescription>> CreatePrescriptionForConsultation(int id, Prescription prescription)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null) return NotFound("Consultation introuvable");

            prescription.ConsultationId = id;
            prescription.DoctorId = consultation.DoctorId;
            prescription.PatientId = consultation.PatientId;

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return Created($"/api/ordonnances/{prescription.Id}", prescription);
        }
    }
}
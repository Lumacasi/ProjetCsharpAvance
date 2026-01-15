using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;
        
        public PatientsController(KingdomHospitalContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            return patient;
        }
        
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetPatient), new {id = patient.Id}, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, Patient patient)
        {
            if (id != patient.Id) return BadRequest("Id doesn't match request");
            
            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Patients.Any(p => p.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        [HttpGet("{id}/consultations")]
        public async Task<ActionResult<IEnumerable<Consultation>>> GetPatientConsultations(int id)
        {
            if (!await _context.Patients.AnyAsync(p => p.Id == id)) return NotFound();
            
            return await _context.Consultations
                .Include(c => c.Doctor)
                .Where(c => c.PatientId == id)
                .ToListAsync();
        }

        [HttpGet("{id}/ordonnances")]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPatientPrescriptions(int id)
        {
            if (!await _context.Patients.AnyAsync(p => p.Id == id)) return NotFound();

            return await _context.Prescriptions
                .Include(p => p.Doctor)
                .Where(p => p.PatientId == id)
                .ToListAsync();
        }
    }
}


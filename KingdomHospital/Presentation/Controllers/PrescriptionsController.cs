using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/ordonnances")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;

        public PrescriptionsController(KingdomHospitalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptions()
        {
            return await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Lines)
                .ThenInclude(l => l.Medicament)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Prescription>> GetPrescription(int id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Lines)
                .ThenInclude(l => l.Medicament)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prescription == null) return NotFound();
            return prescription;
        }

        [HttpPost]
        public async Task<ActionResult<Prescription>> CreatePrescription(Prescription prescription)
        {
            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == prescription.DoctorId);
            var patientExists = await _context.Patients.AnyAsync(p => p.Id == prescription.PatientId);

            if (!doctorExists) return BadRequest("Doctor does not exist");
            if (!patientExists) return BadRequest("Patient does not exist");

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.Id }, prescription);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescreption(int id, Prescription prescription)
        {
            if (id != prescription.Id) return BadRequest("Id doesn't match request");
            
            _context.Entry(prescription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Prescriptions.Any(p => p.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null) return NotFound();
            
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptions(
            [FromQuery] int? doctorId, 
            [FromQuery] int? patientId, 
            [FromQuery] DateOnly? from, 
            [FromQuery] DateOnly? to)
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

        [HttpPut("{id}/consultation/{consultationId}")]
        public async Task<IActionResult> AttachConsultation(int id, int consultationId)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null) return NotFound("Ordonnance introuvable");
            
            if (!await _context.Consultations.AnyAsync(c => c.Id == consultationId)) 
                return BadRequest("Consultation introuvable");

            prescription.ConsultationId = consultationId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}/consultation")]
        public async Task<IActionResult> DetachConsultation(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null) return NotFound();

            prescription.ConsultationId = null; 
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/lignes")]
        public async Task<ActionResult<IEnumerable<PrescriptionLine>>> GetLines(int id)
        {
            if (!await _context.Prescriptions.AnyAsync(p => p.Id == id)) return NotFound();
            
            return await _context.PrescriptionLines
                .Where(l => l.PrescriptionId == id)
                .Include(l => l.Medicament) 
                .ToListAsync();
        }

        [HttpPost("{id}/lignes")]
        public async Task<ActionResult> AddLines(int id, List<PrescriptionLine> lines)
        {
            if (!await _context.Prescriptions.AnyAsync(p => p.Id == id)) return NotFound("Ordonnance introuvable");

            foreach(var line in lines)
            {
                line.PrescriptionId = id; 
                _context.PrescriptionLines.Add(line);
            }
            await _context.SaveChangesAsync();
            return Ok(lines);
        }

        [HttpGet("{id}/lignes/{ligneId}")]
        public async Task<ActionResult<PrescriptionLine>> GetLine(int id, int ligneId)
        {
            var line = await _context.PrescriptionLines
                .Include(l => l.Medicament)
                .FirstOrDefaultAsync(l => l.Id == ligneId && l.PrescriptionId == id);

            if (line == null) return NotFound();
            return line;
        }

        [HttpPut("{id}/lignes/{ligneId}")]
        public async Task<IActionResult> UpdateLine(int id, int ligneId, PrescriptionLine line)
        {
            if (ligneId != line.Id) return BadRequest();
            
            var existingLine = await _context.PrescriptionLines
                .FirstOrDefaultAsync(l => l.Id == ligneId && l.PrescriptionId == id);
            
            if (existingLine == null) return NotFound();

            _context.Entry(existingLine).CurrentValues.SetValues(line);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}/lignes/{ligneId}")]
        public async Task<IActionResult> DeleteLine(int id, int ligneId)
        {
             var line = await _context.PrescriptionLines
                 .FirstOrDefaultAsync(l => l.Id == ligneId && l.PrescriptionId == id);
             
             if (line == null) return NotFound();
             
             _context.PrescriptionLines.Remove(line);
             await _context.SaveChangesAsync();
             return NoContent();
        }
    }
}
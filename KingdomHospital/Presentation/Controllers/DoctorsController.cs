using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;

        public DoctorsController(KingdomHospitalContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            return await _context.Doctors
                .Include(d => d.Specialty)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null) return NotFound();
            return doctor;
        }
        
        [HttpPost]
        public async Task<ActionResult<Doctor>> CreateDoctor(Doctor doctor)
        {
            var specialtyExists = await _context.Specialties.AnyAsync(s => s.Id == doctor.SpecialtyId);
            if (!specialtyExists) return BadRequest("Specialty does not exist");
            
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetDoctor), new {id = doctor.Id}, doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, Doctor doctor)
        {
            if (id != doctor.Id) return BadRequest("Id doesn't match request");

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Doctors.Any(d => d.Id == id)) return NotFound();
                else throw;
            }
            
            return NoContent();
        }
        
        [HttpGet("{id}/specialty")]
        public async Task<ActionResult<Specialty>> GetDoctorSpecialty(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null) return NotFound();
            return Ok(doctor.Specialty);
        }
        
        [HttpPut("{id}/specialty/{specialtyId}")]
        public async Task<IActionResult> UpdateDoctorSpecialty(int id, int specialtyId)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound("Médecin introuvable");

            var specialtyExists = await _context.Specialties.AnyAsync(s => s.Id == specialtyId);
            if (!specialtyExists) return BadRequest("Spécialité introuvable");

            doctor.SpecialtyId = specialtyId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/consultations")]
        public async Task<ActionResult<IEnumerable<Consultation>>> GetDoctorConsultations(int id, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
        {
            var query = _context.Consultations.Where(c => c.DoctorId == id);

            if (from.HasValue) query = query.Where(c => c.Date >= from.Value);
            if (to.HasValue) query = query.Where(c => c.Date <= to.Value);

            return await query.ToListAsync();
        }

        [HttpGet("{id}/patients")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetDoctorPatients(int id)
        {
            var patients = await _context.Consultations
                .Where(c => c.DoctorId == id)
                .Select(c => c.Patient)
                .Distinct()
                .ToListAsync();

            return Ok(patients);
        }

        [HttpGet("{id}/ordonnances")]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetDoctorPrescriptions(int id, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
        {
            var query = _context.Prescriptions.Where(p => p.DoctorId == id);

            if (from.HasValue) query = query.Where(p => p.Date >= from.Value);
            if (to.HasValue) query = query.Where(p => p.Date <= to.Value);

            return await query.ToListAsync();
        }
    }
}
using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtiesController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;

        public SpecialtiesController(KingdomHospitalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialty>>> GetSpecialties()
        {
            return await _context.Specialties.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Specialty>> GetSpecialty(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null) return NotFound();
            return specialty;
        }

        [HttpGet("{id}/doctors")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsBySpecialty(int id)
        {
            var specialty = await _context.Specialties
                .Include(s => s.Doctors)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (specialty == null) return NotFound();
            
            return Ok(specialty.Doctors);
        }
    }
}
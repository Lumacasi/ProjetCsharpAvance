using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicamentsController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;
        
        public MedicamentsController(KingdomHospitalContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicament>>> GetMedicaments()
        {
            return await _context.Medicaments.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Medicament>> GetMedicament(int id)
        {
            var medicament = await _context.Medicaments.FindAsync(id);
            if (medicament == null) return NotFound();
            return medicament;
        }
        
        [HttpPost]
        public async Task<ActionResult<Medicament>> CreateMedicament(Medicament medicament)
        {
            if (await _context.Medicaments.AnyAsync(m => m.Name == medicament.Name))
            {
                return BadRequest("Medicament already exists");
            }
            
            _context.Medicaments.Add(medicament);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetMedicament), new {id = medicament.Id}, medicament);
        }
        
        [HttpGet("{id}/ordonnances")]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptionsByMedicament(int id)
        {
            if (!await _context.Medicaments.AnyAsync(m => m.Id == id)) return NotFound();

            var prescriptions = await _context.Prescriptions
                .Include(p => p.Lines)
                .Where(p => p.Lines.Any(l => l.MedicamentId == id))
                .ToListAsync();

            return Ok(prescriptions);
        }
    }
}
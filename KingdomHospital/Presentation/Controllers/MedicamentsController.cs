using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
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
        private readonly MedicamentMapper _mapper;
        private readonly PrescriptionMapper _prescriptionMapper; // Pour l'endpoint relationnel

        public MedicamentsController(KingdomHospitalContext context, MedicamentMapper mapper, PrescriptionMapper prescriptionMapper)
        {
            _context = context;
            _mapper = mapper;
            _prescriptionMapper = prescriptionMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicamentDto>>> GetMedicaments()
        {
            var meds = await _context.Medicaments.ToListAsync();
            return Ok(meds.Select(m => _mapper.ToDto(m)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicamentDto>> GetMedicament(int id)
        {
            var med = await _context.Medicaments.FindAsync(id);
            if (med == null) return NotFound();
            return Ok(_mapper.ToDto(med));
        }

        [HttpPost]
        public async Task<ActionResult<MedicamentDto>> CreateMedicament(CreateMedicamentDto dto)
        {
            if (await _context.Medicaments.AnyAsync(m => m.Name == dto.Name))
                return BadRequest("Ce médicament existe déjà.");

            var med = _mapper.ToEntity(dto);
            _context.Medicaments.Add(med);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicament), new { id = med.Id }, _mapper.ToDto(med));
        }

        // Relationnel : Où ce médicament est-il utilisé ?
        [HttpGet("{id}/ordonnances")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptionsByMedicament(int id)
        {
            if (!await _context.Medicaments.AnyAsync(m => m.Id == id)) return NotFound();

            var prescriptions = await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Lines).ThenInclude(l => l.Medicament)
                .Where(p => p.Lines.Any(l => l.MedicamentId == id))
                .ToListAsync();

            return Ok(prescriptions.Select(p => _prescriptionMapper.ToDto(p)));
        }
    }
}
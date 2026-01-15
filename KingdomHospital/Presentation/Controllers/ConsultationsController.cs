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
    public class ConsultationsController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;
        private readonly ConsultationMapper _mapper;

        public ConsultationsController(KingdomHospitalContext context, ConsultationMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetConsultations()
        {
            var consultations = await _context.Consultations
                .Include(c => c.Doctor)  
                .Include(c => c.Patient)
                .ToListAsync();

            return Ok(consultations.Select(c => _mapper.ToDto(c)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultationDto>> GetConsultation(int id)
        {
            var consultation = await _context.Consultations
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consultation == null) return NotFound();
            return Ok(_mapper.ToDto(consultation));
        }

        [HttpPost]
        public async Task<ActionResult<ConsultationDto>> CreateConsultation(CreateConsultationDto dto)
        {
            if (!await _context.Doctors.AnyAsync(d => d.Id == dto.DoctorId) ||
                !await _context.Patients.AnyAsync(p => p.Id == dto.PatientId))
            {
                return BadRequest("Docteur ou Patient inconnu");
            }

            var consultation = _mapper.ToEntity(dto);
            _context.Consultations.Add(consultation);
            await _context.SaveChangesAsync();

            await _context.Entry(consultation).Reference(c => c.Doctor).LoadAsync();
            await _context.Entry(consultation).Reference(c => c.Patient).LoadAsync();

            return CreatedAtAction(nameof(GetConsultation), new { id = consultation.Id }, _mapper.ToDto(consultation));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsultation(int id, CreateConsultationDto dto)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null) return NotFound();

            _mapper.UpdateEntity(dto, consultation);
            await _context.SaveChangesAsync();
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
    }
}
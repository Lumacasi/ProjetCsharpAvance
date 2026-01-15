using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationsController : ControllerBase
    {
        private readonly ConsultationService _service;

        public ConsultationsController(ConsultationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetConsultations(
            [FromQuery] int? doctorId,
            [FromQuery] int? patientId,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to)
        {
            return Ok(await _service.GetAllConsultationsAsync(doctorId, patientId, from, to));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultationDto>> GetConsultation(int id)
        {
            var consultation = await _service.GetConsultationByIdAsync(id);
            if (consultation == null) return NotFound();
            return Ok(consultation);
        }

        [HttpPost]
        public async Task<ActionResult<ConsultationDto>> CreateConsultation(CreateConsultationDto dto)
        {
            var created = await _service.CreateConsultationAsync(dto);
            if (created == null) return BadRequest("Médecin ou Patient introuvable");
            return CreatedAtAction(nameof(GetConsultation), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsultation(int id, CreateConsultationDto dto)
        {
            var success = await _service.UpdateConsultationAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsultation(int id)
        {
            var success = await _service.DeleteConsultationAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
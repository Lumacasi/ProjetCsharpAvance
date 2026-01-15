using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/ordonnances")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly PrescriptionService _service;

        public PrescriptionsController(PrescriptionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptions(
            [FromQuery] int? doctorId,
            [FromQuery] int? patientId,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to)
        {
            return Ok(await _service.GetAllPrescriptionsAsync(doctorId, patientId, from, to));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDto>> GetPrescription(int id)
        {
            var prescription = await _service.GetPrescriptionByIdAsync(id);
            if (prescription == null) return NotFound();
            return Ok(prescription);
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionDto>> CreatePrescription(CreatePrescriptionDto dto)
        {
            var created = await _service.CreatePrescriptionAsync(dto);
            if (created == null) return BadRequest("Médecin ou Patient introuvable.");
            return CreatedAtAction(nameof(GetPrescription), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(int id, CreatePrescriptionDto dto)
        {
            var success = await _service.UpdatePrescriptionAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var success = await _service.DeletePrescriptionAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/consultation/{consultationId}")]
        public async Task<IActionResult> AttachConsultation(int id, int consultationId)
        {
            var success = await _service.AttachConsultationAsync(id, consultationId);
            if (!success) return BadRequest("Erreur lors du rattachement (IDs invalides).");
            return NoContent();
        }

        [HttpDelete("{id}/consultation")]
        public async Task<IActionResult> DetachConsultation(int id)
        {
            var success = await _service.DetachConsultationAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/lignes")]
        public async Task<ActionResult<IEnumerable<PrescriptionLineDto>>> GetLines(int id)
        {
            var lines = await _service.GetLinesAsync(id);
            if (lines == null) return NotFound();
            return Ok(lines);
        }

        [HttpPost("{id}/lignes")]
        public async Task<ActionResult> AddLines(int id, List<CreatePrescriptionLineDto> linesDto)
        {
            var success = await _service.AddLinesAsync(id, linesDto);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpPut("{id}/lignes/{ligneId}")]
        public async Task<IActionResult> UpdateLine(int id, int ligneId, CreatePrescriptionLineDto dto)
        {
            var success = await _service.UpdateLineAsync(id, ligneId, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}/lignes/{ligneId}")]
        public async Task<IActionResult> DeleteLine(int id, int ligneId)
        {
            var success = await _service.DeleteLineAsync(id, ligneId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
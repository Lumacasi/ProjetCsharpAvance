using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using KingdomHospital.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _service;

        public PatientsController(PatientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
        {
            return Ok(await _service.GetAllPatientsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int id)
        {
            var patient = await _service.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient(CreatePatientDto dto)
        {
            var created = await _service.CreatePatientAsync(dto);
            return CreatedAtAction(nameof(GetPatient), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, CreatePatientDto dto)
        {
            var success = await _service.UpdatePatientAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var success = await _service.DeletePatientAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/consultations")]
        public async Task<ActionResult<IEnumerable<Consultation>>> GetConsultations(int id)
        {
            return Ok(await _service.GetConsultationsAsync(id));
        }

        [HttpGet("{id}/ordonnances")]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptions(int id)
        {
            return Ok(await _service.GetPrescriptionsAsync(id));
        }
    }
}
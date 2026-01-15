using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using KingdomHospital.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorService _service;

        public DoctorsController(DoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
        {
            return Ok(await _service.GetAllDoctorsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
            var doctor = await _service.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        [HttpPost]
        public async Task<ActionResult<DoctorDto>> CreateDoctor(CreateDoctorDto dto)
        {
            var created = await _service.CreateDoctorAsync(dto);
            return CreatedAtAction(nameof(GetDoctor), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, CreateDoctorDto dto)
        {
            var success = await _service.UpdateDoctorAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/consultations")]
        public async Task<ActionResult<IEnumerable<Consultation>>> GetConsultations(int id, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
        {
            return Ok(await _service.GetConsultationsAsync(id, from, to));
        }
    }
}
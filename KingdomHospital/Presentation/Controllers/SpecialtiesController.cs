using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtiesController : ControllerBase
    {
        private readonly SpecialtyService _service;

        public SpecialtiesController(SpecialtyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecialtyDto>>> GetSpecialties()
        {
            return Ok(await _service.GetAllSpecialtiesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialtyDto>> GetSpecialty(int id)
        {
            var specialty = await _service.GetSpecialtyByIdAsync(id);
            if (specialty == null) return NotFound();
            return Ok(specialty);
        }

        [HttpGet("{id}/doctors")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsBySpecialty(int id)
        {
            var doctors = await _service.GetDoctorsBySpecialtyIdAsync(id);
            if (doctors == null) return NotFound();
            return Ok(doctors);
        }
    }
}
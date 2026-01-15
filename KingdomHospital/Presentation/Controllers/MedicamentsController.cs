using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicamentsController : ControllerBase
    {
        private readonly MedicamentService _service;

        public MedicamentsController(MedicamentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicamentDto>>> GetMedicaments()
        {
            return Ok(await _service.GetAllMedicamentsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicamentDto>> GetMedicament(int id)
        {
            var medicament = await _service.GetMedicamentByIdAsync(id);
            if (medicament == null) return NotFound();
            return Ok(medicament);
        }

        [HttpPost]
        public async Task<ActionResult<MedicamentDto>> CreateMedicament(CreateMedicamentDto dto)
        {
            var created = await _service.CreateMedicamentAsync(dto);
            if (created == null) return BadRequest("Ce médicament existe déjà.");
            return CreatedAtAction(nameof(GetMedicament), new { id = created.Id }, created);
        }

        [HttpGet("{id}/ordonnances")]
        public async Task<ActionResult> GetPrescriptionsByMedicament(int id)
        {
            if (await _service.GetMedicamentByIdAsync(id) == null) return NotFound();
            return Ok(await _service.GetPrescriptionsByMedicamentAsync(id));
        }
    }
}
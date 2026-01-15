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
    public class DoctorsController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;
        private readonly DoctorMapper _mapper; // <-- On injecte le mapper

        public DoctorsController(KingdomHospitalContext context, DoctorMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
        {
            var doctors = await _context.Doctors
                                 .Include(d => d.Specialty) // Toujours besoin de l'include pour le mapping !
                                 .ToListAsync();
            
            // On convertit la liste d'entités en liste de DTOs
            // .Select(d => _mapper.ToDto(d)) transforme chaque médecin
            return Ok(doctors.Select(d => _mapper.ToDto(d)));
        }

        // GET: api/doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors
                                       .Include(d => d.Specialty)
                                       .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null) return NotFound();

            // On renvoie le DTO propre
            return Ok(_mapper.ToDto(doctor));
        }

        // POST: api/doctors
        // On reçoit un CreateDoctorDto, pas un Doctor !
        [HttpPost]
        public async Task<ActionResult<DoctorDto>> CreateDoctor(CreateDoctorDto doctorDto)
        {
            // Validation manuelle de la FK (bonne pratique)
            if (!await _context.Specialties.AnyAsync(s => s.Id == doctorDto.SpecialtyId))
            {
                return BadRequest("SpecialtyId invalide.");
            }

            // 1. DTO -> Entité
            var doctor = _mapper.ToEntity(doctorDto);

            // 2. Sauvegarde
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            // 3. On doit recharger la spécialité pour renvoyer un joli DTO complet
            await _context.Entry(doctor).Reference(d => d.Specialty).LoadAsync();

            // 4. Entité -> DTO pour la réponse
            var responseDto = _mapper.ToDto(doctor);

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, responseDto);
        }

        // PUT: api/doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, CreateDoctorDto doctorDto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            // Le mapper met à jour l'entité existante avec les nouvelles valeurs
            _mapper.UpdateEntity(doctorDto, doctor);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               throw;
            }

            return NoContent();
        }
    }
}
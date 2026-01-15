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
    public class SpecialtiesController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;
        private readonly SpecialtyMapper _mapper;
        private readonly DoctorMapper _doctorMapper; 

        public SpecialtiesController(KingdomHospitalContext context, SpecialtyMapper mapper, DoctorMapper doctorMapper)
        {
            _context = context;
            _mapper = mapper;
            _doctorMapper = doctorMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecialtyDto>>> GetSpecialties()
        {
            var specialties = await _context.Specialties.ToListAsync();
            return Ok(specialties.Select(s => _mapper.ToDto(s)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialtyDto>> GetSpecialty(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null) return NotFound();
            return Ok(_mapper.ToDto(specialty));
        }

        [HttpGet("{id}/doctors")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsBySpecialty(int id)
        {
            var specialty = await _context.Specialties
                .Include(s => s.Doctors).ThenInclude(d => d.Specialty) 
                .FirstOrDefaultAsync(s => s.Id == id);

            if (specialty == null) return NotFound();
            
            return Ok(specialty.Doctors.Select(d => _doctorMapper.ToDto(d)));
        }
    }
}
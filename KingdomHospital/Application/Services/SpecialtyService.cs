using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;

namespace KingdomHospital.Application.Services
{
    public class SpecialtyService
    {
        private readonly ISpecialtyRepository _repository;
        private readonly SpecialtyMapper _mapper;
        private readonly DoctorMapper _doctorMapper;

        public SpecialtyService(ISpecialtyRepository repository, SpecialtyMapper mapper, DoctorMapper doctorMapper)
        {
            _repository = repository;
            _mapper = mapper;
            _doctorMapper = doctorMapper;
        }

        public async Task<IEnumerable<SpecialtyDto>> GetAllSpecialtiesAsync()
        {
            var specialties = await _repository.GetAllAsync();
            return specialties.Select(s => _mapper.ToDto(s));
        }

        public async Task<SpecialtyDto?> GetSpecialtyByIdAsync(int id)
        {
            var specialty = await _repository.GetByIdAsync(id);
            return specialty == null ? null : _mapper.ToDto(specialty);
        }

        public async Task<IEnumerable<DoctorDto>?> GetDoctorsBySpecialtyIdAsync(int id)
        {
            var specialty = await _repository.GetByIdWithDoctorsAsync(id);
            if (specialty == null) return null;

            return specialty.Doctors.Select(d => _doctorMapper.ToDto(d));
        }
    }
}
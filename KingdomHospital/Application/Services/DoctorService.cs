using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly DoctorMapper _mapper;

        public DoctorService(IDoctorRepository repository, DoctorMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _repository.GetAllAsync();
            return doctors.Select(d => _mapper.ToDto(d));
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            return doctor == null ? null : _mapper.ToDto(doctor);
        }

        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto)
        {
            var doctor = _mapper.ToEntity(dto);
            await _repository.AddAsync(doctor);
            var loadedDoctor = await _repository.GetByIdAsync(doctor.Id); 
            return _mapper.ToDto(loadedDoctor!);
        }

        public async Task<bool> UpdateDoctorAsync(int id, CreateDoctorDto dto)
        {
            var existingDoctor = await _repository.GetByIdAsync(id);
            if (existingDoctor == null) return false;

            _mapper.UpdateEntity(dto, existingDoctor);
            await _repository.UpdateAsync(existingDoctor);
            return true;
        }
        
        public async Task<IEnumerable<Consultation>> GetConsultationsAsync(int id, DateOnly? from, DateOnly? to)
        {
            return await _repository.GetConsultationsAsync(id, from, to);
        }
    }
}
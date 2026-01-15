using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Services
{
    public class ConsultationService
    {
        private readonly IConsultationRepository _repository;
        private readonly ConsultationMapper _mapper;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IPatientRepository _patientRepo;

        public ConsultationService(IConsultationRepository repository, ConsultationMapper mapper, 
                                   IDoctorRepository doctorRepo, IPatientRepository patientRepo)
        {
            _repository = repository;
            _mapper = mapper;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
        }

        public async Task<IEnumerable<ConsultationDto>> GetAllConsultationsAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to)
        {
            var consultations = await _repository.GetAllAsync(doctorId, patientId, from, to);
            return consultations.Select(c => _mapper.ToDto(c));
        }

        public async Task<ConsultationDto?> GetConsultationByIdAsync(int id)
        {
            var consultation = await _repository.GetByIdAsync(id);
            return consultation == null ? null : _mapper.ToDto(consultation);
        }

        public async Task<ConsultationDto?> CreateConsultationAsync(CreateConsultationDto dto)
        {
            var doctor = await _doctorRepo.GetByIdAsync(dto.DoctorId);
            var patientExists = await _patientRepo.ExistsAsync(dto.PatientId);

            if (doctor == null || !patientExists) return null;

            var consultation = _mapper.ToEntity(dto);
            await _repository.AddAsync(consultation);
            
            var loaded = await _repository.GetByIdAsync(consultation.Id);
            return _mapper.ToDto(loaded!);
        }

        public async Task<bool> UpdateConsultationAsync(int id, CreateConsultationDto dto)
        {
            var consultation = await _repository.GetByIdAsync(id);
            if (consultation == null) return false;

            _mapper.UpdateEntity(dto, consultation);
            await _repository.UpdateAsync(consultation);
            return true;
        }

        public async Task<bool> DeleteConsultationAsync(int id)
        {
            var consultation = await _repository.GetByIdAsync(id);
            if (consultation == null) return false;

            await _repository.DeleteAsync(consultation);
            return true;
        }
    }
}
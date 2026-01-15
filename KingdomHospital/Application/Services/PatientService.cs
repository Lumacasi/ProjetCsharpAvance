using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _repository;
        private readonly PatientMapper _mapper;
        private readonly IConsultationRepository _consultationRepo;
        private readonly IPrescriptionRepository _prescriptionRepo;

        public PatientService(IPatientRepository repository, PatientMapper mapper, 
                              IConsultationRepository consultationRepo, IPrescriptionRepository prescriptionRepo)
        {
            _repository = repository;
            _mapper = mapper;
            _consultationRepo = consultationRepo;
            _prescriptionRepo = prescriptionRepo;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _repository.GetAllAsync();
            return patients.Select(p => _mapper.ToDto(p));
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            return patient == null ? null : _mapper.ToDto(patient);
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto dto)
        {
            var patient = _mapper.ToEntity(dto);
            await _repository.AddAsync(patient);
            return _mapper.ToDto(patient);
        }

        public async Task<bool> UpdatePatientAsync(int id, CreatePatientDto dto)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return false;

            _mapper.UpdateEntity(dto, patient);
            await _repository.UpdateAsync(patient);
            return true;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return false;

            await _repository.DeleteAsync(patient);
            return true;
        }

        public async Task<IEnumerable<Consultation>> GetConsultationsAsync(int patientId)
        {
            return await _consultationRepo.GetAllAsync(null, patientId, null, null);
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsAsync(int patientId)
        {
            return await _prescriptionRepo.GetAllAsync(null, patientId, null, null);
        }
    }
}
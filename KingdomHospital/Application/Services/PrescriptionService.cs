using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Services
{
    public class PrescriptionService
    {
        private readonly IPrescriptionRepository _repository;
        private readonly PrescriptionMapper _mapper;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IPatientRepository _patientRepo;
        private readonly IConsultationRepository _consultationRepo;

        public PrescriptionService(IPrescriptionRepository repository, PrescriptionMapper mapper,
                                   IDoctorRepository doctorRepo, IPatientRepository patientRepo, 
                                   IConsultationRepository consultationRepo)
        {
            _repository = repository;
            _mapper = mapper;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
            _consultationRepo = consultationRepo;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to)
        {
            var list = await _repository.GetAllAsync(doctorId, patientId, from, to);
            return list.Select(p => _mapper.ToDto(p));
        }

        public async Task<PrescriptionDto?> GetPrescriptionByIdAsync(int id)
        {
            var prescription = await _repository.GetByIdAsync(id);
            return prescription == null ? null : _mapper.ToDto(prescription);
        }

        public async Task<PrescriptionDto?> CreatePrescriptionAsync(CreatePrescriptionDto dto)
        {
            var doctor = await _doctorRepo.GetByIdAsync(dto.DoctorId);
            var patient = await _patientRepo.GetByIdAsync(dto.PatientId);

            if (doctor == null || patient == null) return null;

            var prescription = _mapper.ToEntity(dto);
            
            if (dto.Lines != null)
            {
                foreach (var lineDto in dto.Lines)
                {
                    var line = _mapper.LineToEntity(lineDto);
                    prescription.Lines.Add(line);
                }
            }

            await _repository.AddAsync(prescription);

            var loaded = await _repository.GetByIdAsync(prescription.Id);
            return _mapper.ToDto(loaded!);
        }

        public async Task<bool> UpdatePrescriptionAsync(int id, CreatePrescriptionDto dto)
        {
            var prescription = await _repository.GetByIdAsync(id);
            if (prescription == null) return false;

            prescription.Date = dto.Date;
            prescription.Notes = dto.Notes;
            prescription.DoctorId = dto.DoctorId;
            prescription.PatientId = dto.PatientId;
            prescription.ConsultationId = dto.ConsultationId;

            await _repository.UpdateAsync(prescription);
            return true;
        }

        public async Task<bool> DeletePrescriptionAsync(int id)
        {
            var prescription = await _repository.GetByIdAsync(id);
            if (prescription == null) return false;

            await _repository.DeleteAsync(prescription);
            return true;
        }
        
        public async Task<bool> AttachConsultationAsync(int id, int consultationId)
        {
            var prescription = await _repository.GetByIdAsync(id);
            if (prescription == null) return false;

            if (!await _consultationRepo.ExistsAsync(consultationId)) return false;

            prescription.ConsultationId = consultationId;
            await _repository.UpdateAsync(prescription);
            return true;
        }

        public async Task<bool> DetachConsultationAsync(int id)
        {
            var prescription = await _repository.GetByIdAsync(id);
            if (prescription == null) return false;

            prescription.ConsultationId = null;
            await _repository.UpdateAsync(prescription);
            return true;
        }
        
        public async Task<IEnumerable<PrescriptionLineDto>?> GetLinesAsync(int prescriptionId)
        {
            if (await _repository.GetByIdAsync(prescriptionId) == null) return null;

            var lines = await _repository.GetLinesAsync(prescriptionId);
            return lines.Select(l => _mapper.LineToDto(l));
        }

        public async Task<bool> AddLinesAsync(int prescriptionId, List<CreatePrescriptionLineDto> linesDto)
        {
            if (await _repository.GetByIdAsync(prescriptionId) == null) return false;

            foreach (var dto in linesDto)
            {
                var line = _mapper.LineToEntity(dto);
                line.PrescriptionId = prescriptionId;
                await _repository.AddLineAsync(line);
            }
            return true;
        }

        public async Task<bool> UpdateLineAsync(int prescriptionId, int lineId, CreatePrescriptionLineDto dto)
        {
            var line = await _repository.GetLineByIdAsync(prescriptionId, lineId);
            if (line == null) return false;

            line.MedicamentId = dto.MedicamentId;
            line.Dosage = dto.Dosage;
            line.Duration = dto.Duration;
            line.Frequency = dto.Frequency;
            line.Quantity = dto.Quantity;
            line.Instructions = dto.Instructions;

            await _repository.UpdateLineAsync(line);
            return true;
        }

        public async Task<bool> DeleteLineAsync(int prescriptionId, int lineId)
        {
            var line = await _repository.GetLineByIdAsync(prescriptionId, lineId);
            if (line == null) return false;

            await _repository.DeleteLineAsync(line);
            return true;
        }
    }
}
using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Services
{
    public class MedicamentService
    {
        private readonly IMedicamentRepository _repository;
        private readonly MedicamentMapper _mapper;
        private readonly IPrescriptionRepository _prescriptionRepo;

        public MedicamentService(IMedicamentRepository repository, MedicamentMapper mapper, IPrescriptionRepository prescriptionRepo)
        {
            _repository = repository;
            _mapper = mapper;
            _prescriptionRepo = prescriptionRepo;
        }

        public async Task<IEnumerable<MedicamentDto>> GetAllMedicamentsAsync()
        {
            var meds = await _repository.GetAllAsync();
            return meds.Select(m => _mapper.ToDto(m));
        }

        public async Task<MedicamentDto?> GetMedicamentByIdAsync(int id)
        {
            var med = await _repository.GetByIdAsync(id);
            return med == null ? null : _mapper.ToDto(med);
        }

        public async Task<MedicamentDto?> CreateMedicamentAsync(CreateMedicamentDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name)) return null;

            var med = _mapper.ToEntity(dto);
            await _repository.AddAsync(med);
            return _mapper.ToDto(med);
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByMedicamentAsync(int medicamentId)
        {
            var allPrescriptions = await _prescriptionRepo.GetAllAsync(null, null, null, null);
            return allPrescriptions.Where(p => p.Lines.Any(l => l.MedicamentId == medicamentId));
        }
    }
}
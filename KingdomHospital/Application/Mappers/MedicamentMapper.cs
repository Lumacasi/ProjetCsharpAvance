using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers
{
    [Mapper]
    public partial class MedicamentMapper
    {
        public partial MedicamentDto ToDto(Medicament medicament);

        [MapperIgnoreTarget(nameof(Medicament.Id))]
        [MapperIgnoreTarget(nameof(Medicament.PrescriptionLines))]
        public partial Medicament ToEntity(CreateMedicamentDto dto);
    }
}
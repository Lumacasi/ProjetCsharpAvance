using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers
{
    [Mapper]
    public partial class SpecialtyMapper
    {
        public partial SpecialtyDto ToDto(Specialty specialty);

        [MapperIgnoreTarget(nameof(Specialty.Id))]
        [MapperIgnoreTarget(nameof(Specialty.Doctors))]
        public partial Specialty ToEntity(CreateSpecialtyDto dto);
        
        [MapperIgnoreTarget(nameof(Specialty.Id))]
        [MapperIgnoreTarget(nameof(Specialty.Doctors))]
        public partial void UpdateEntity(CreateSpecialtyDto dto, Specialty specialty);
    }
}
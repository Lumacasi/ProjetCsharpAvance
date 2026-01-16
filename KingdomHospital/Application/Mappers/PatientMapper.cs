using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers
{
    [Mapper]
    public partial class PatientMapper
    {
        public partial PatientDto ToDto(Patient patient);
        
        [MapProperty(nameof(Patient.FirstName), nameof(PatientDto.FirstName))] 
        [MapProperty(nameof(Patient.LastName), nameof(PatientDto.LastName))]
        


        [MapperIgnoreTarget(nameof(Patient.Id))]
        [MapperIgnoreTarget(nameof(Patient.Consultations))]
        public partial Patient ToEntity(CreatePatientDto dto);

        [MapperIgnoreTarget(nameof(Patient.Id))]
        [MapperIgnoreTarget(nameof(Patient.Consultations))]
        public partial void UpdateEntity(CreatePatientDto dto, Patient patient);
    }
}
using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers
{
    [Mapper]
    public partial class PatientMapper
    {
        [MapProperty(nameof(Patient), nameof(PatientDto.FullName), Use = nameof(MapFullName))]
        public partial PatientDto ToDto(Patient patient);

        private string MapFullName(Patient patient) => $"{patient.FirstName} {patient.LastName}";

        [MapperIgnoreTarget(nameof(Patient.Id))]
        [MapperIgnoreTarget(nameof(Patient.Consultations))]
        public partial Patient ToEntity(CreatePatientDto dto);

        [MapperIgnoreTarget(nameof(Patient.Id))]
        [MapperIgnoreTarget(nameof(Patient.Consultations))]
        public partial void UpdateEntity(CreatePatientDto dto, Patient patient);
    }
}
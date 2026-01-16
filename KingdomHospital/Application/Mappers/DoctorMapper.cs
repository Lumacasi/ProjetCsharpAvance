using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers
{
    [Mapper]
    public partial class DoctorMapper
    {
        [MapProperty(nameof(Doctor.Specialty.Name), nameof(DoctorDto.SpecialtyName))] 
        public partial DoctorDto ToDto(Doctor doctor);
        
        [MapProperty(nameof(Doctor.FirstName), nameof(DoctorDto.FirstName))] 
        [MapProperty(nameof(Doctor.LastName), nameof(DoctorDto.LastName))] 

        private partial DoctorDto DoctorToDtoInternal(Doctor doctor);

        [MapperIgnoreTarget(nameof(Doctor.Id))]
        [MapperIgnoreTarget(nameof(Doctor.Specialty))]
        [MapperIgnoreTarget(nameof(Doctor.Consultations))]
        [MapperIgnoreTarget(nameof(Doctor.Prescriptions))]
        public partial Doctor ToEntity(CreateDoctorDto dto);

        [MapperIgnoreTarget(nameof(Doctor.Id))]
        [MapperIgnoreTarget(nameof(Doctor.Specialty))]
        [MapperIgnoreTarget(nameof(Doctor.Consultations))]
        [MapperIgnoreTarget(nameof(Doctor.Prescriptions))]
        public partial void UpdateEntity(CreateDoctorDto dto, Doctor doctor);
    }
}
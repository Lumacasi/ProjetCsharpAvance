using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers
{
    [Mapper]
    public partial class ConsultationMapper
    {
        [MapProperty(nameof(Consultation.Doctor.LastName), nameof(ConsultationDto.DoctorName))] 
        [MapProperty(nameof(Consultation.Patient.LastName), nameof(ConsultationDto.PatientName))]
        public partial ConsultationDto ToDto(Consultation consultation);

        [MapperIgnoreTarget(nameof(Consultation.Id))]
        [MapperIgnoreTarget(nameof(Consultation.Doctor))]
        [MapperIgnoreTarget(nameof(Consultation.Patient))]
        [MapperIgnoreTarget(nameof(Consultation.Prescriptions))]
        public partial Consultation ToEntity(CreateConsultationDto dto);
        
        [MapperIgnoreTarget(nameof(Consultation.Id))]
        [MapperIgnoreTarget(nameof(Consultation.Doctor))]
        [MapperIgnoreTarget(nameof(Consultation.Patient))]
        [MapperIgnoreTarget(nameof(Consultation.Prescriptions))]
        public partial void UpdateEntity(CreateConsultationDto dto, Consultation consultation);
    }
}
using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers
{
    [Mapper]
    public partial class PrescriptionMapper
    {
        [MapProperty(nameof(Prescription.Doctor.LastName), nameof(PrescriptionDto.DoctorName))]
        [MapProperty(nameof(Prescription.Patient.LastName), nameof(PrescriptionDto.PatientName))]
        public partial PrescriptionDto ToDto(Prescription prescription);

        [MapperIgnoreTarget(nameof(Prescription.Id))]
        [MapperIgnoreTarget(nameof(Prescription.Doctor))]
        [MapperIgnoreTarget(nameof(Prescription.Patient))]
        [MapperIgnoreTarget(nameof(Prescription.Consultation))]
        public partial Prescription ToEntity(CreatePrescriptionDto dto);
        
        [MapProperty(nameof(PrescriptionLine.Medicament.Name), nameof(PrescriptionLineDto.MedicamentName))]
        public partial PrescriptionLineDto LineToDto(PrescriptionLine line);

        [MapperIgnoreTarget(nameof(PrescriptionLine.Id))]
        [MapperIgnoreTarget(nameof(PrescriptionLine.PrescriptionId))]
        [MapperIgnoreTarget(nameof(PrescriptionLine.Medicament))]
        public partial PrescriptionLine LineToEntity(CreatePrescriptionLineDto dto);
    }
}
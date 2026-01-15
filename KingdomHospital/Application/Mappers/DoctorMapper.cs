using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers
{
    // [Mapper] indique à la librairie de générer le code magique derrière
    [Mapper]
    public partial class DoctorMapper
    {
        // 1. Transformer Entité -> DTO (Lecture)
        // On configure le mapping manuel pour concaténer le nom et récupérer le nom de la spé
        [MapProperty(nameof(Doctor.Specialty.Name), nameof(DoctorDto.SpecialtyName))] 
        public partial DoctorDto ToDto(Doctor doctor);
        
        // Petite méthode custom pour le FullName (car Mapperly ne devine pas la concaténation tout seul)
        private string MapFullName(Doctor doctor) => $"{doctor.FirstName} {doctor.LastName}";
        // On dit à Mapperly d'utiliser cette méthode
        [MapProperty(nameof(Doctor), nameof(DoctorDto.FullName), Use = nameof(MapFullName))] 
        private partial DoctorDto DoctorToDtoInternal(Doctor doctor);


        // 2. Transformer DTO -> Entité (Création)
        // On ignore l'ID (généré par la DB) et les relations de navigation
        [MapperIgnoreTarget(nameof(Doctor.Id))]
        [MapperIgnoreTarget(nameof(Doctor.Specialty))]
        [MapperIgnoreTarget(nameof(Doctor.Consultations))]
        [MapperIgnoreTarget(nameof(Doctor.Prescriptions))]
        public partial Doctor ToEntity(CreateDoctorDto dto);

        // 3. Mettre à jour une entité existante avec un DTO
        [MapperIgnoreTarget(nameof(Doctor.Id))]
        [MapperIgnoreTarget(nameof(Doctor.Specialty))]
        [MapperIgnoreTarget(nameof(Doctor.Consultations))]
        [MapperIgnoreTarget(nameof(Doctor.Prescriptions))]
        public partial void UpdateEntity(CreateDoctorDto dto, Doctor doctor);
    }
}
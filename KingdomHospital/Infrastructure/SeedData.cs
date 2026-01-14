using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(KingdomHospitalContext context)
        {
            // if (context.Specialties.Any()) return;

            var baseDir = AppContext.BaseDirectory;
            
            var folderName = "Resources"; 
            var pathSpecialties = Path.Combine(baseDir, "Infrastructure", folderName, "Specialties.csv");
            var pathMedicaments = Path.Combine(baseDir, "Infrastructure", folderName, "Medicaments.csv");
            
            
            
            if (!context.Specialties.Any())
            {
                var specialtiesLines = File.ReadAllLines(pathSpecialties);
                var specialtiesList = new List<Specialty>();

                for (int i = 0; i < specialtiesLines.Length; i++)
                {
                    var line = specialtiesLines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(',');
                    
                    if (parts.Length >= 2)
                    {
                        var name = parts[1].Trim();
                        specialtiesList.Add(new Specialty { Name = name });
                    }
                }
                context.Specialties.AddRange(specialtiesList);
                context.SaveChanges();
            }
            
            
            
            if (!context.Medicaments.Any())
            {
                var medsLines = File.ReadAllLines(pathMedicaments);
                var medsList = new List<Medicament>();
                
                for (int i = 0; i < medsLines.Length; i++)
                {
                    var line = medsLines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(',');
                    if (parts.Length >= 5)
                    {
                        medsList.Add(new Medicament
                        {
                            Name = parts[1].Trim(),
                            DosageForm = parts[2].Trim(),
                            Strength = parts[3].Trim(),
                            AtcCode = parts[4].Trim()
                        });
                    }
                }
                context.Medicaments.AddRange(medsList);
                context.SaveChanges();
            }

            var neuro = context.Specialties.FirstOrDefault(s => s.Name == "Neurologie");
            var cardio = context.Specialties.FirstOrDefault(s => s.Name == "Cardiologie");
            var dermato = context.Specialties.FirstOrDefault(s => s.Name == "Dermatologie");
            var ophta = context.Specialties.FirstOrDefault(s => s.Name == "Ophtalmologie");
            var urgence = context.Specialties.FirstOrDefault(s => s.Name == "Médecine d'Urgence") 
                          ?? context.Specialties.First(); 

            if (neuro == null || cardio == null) return;


            
            if (!context.Doctors.Any())
            {
                var doctors = new List<Doctor>
                {
                    new Doctor { FirstName = "Gregory", LastName = "House", Specialty = neuro },
                    new Doctor { FirstName = "Derek", LastName = "Shepherd", Specialty = neuro },
                    new Doctor { FirstName = "Meredith", LastName = "Grey", Specialty = cardio },
                    new Doctor { FirstName = "Cristina", LastName = "Yang", Specialty = cardio },
                    new Doctor { FirstName = "Miranda", LastName = "Bailey", Specialty = dermato },
                    new Doctor { FirstName = "Stephen", LastName = "Strange", Specialty = ophta },
                    new Doctor { FirstName = "John", LastName = "Carter", Specialty = urgence }
                };
                context.Doctors.AddRange(doctors);
                context.SaveChanges();
            }
            
          
            
            if (!context.Patients.Any())
            {
                var patients = new List<Patient>
                {
                    new Patient { FirstName = "John", LastName = "Doe", BirthDate = new DateOnly(1985, 1, 1) },
                    new Patient { FirstName = "Jane", LastName = "Smith", BirthDate = new DateOnly(1992, 5, 20) },
                    new Patient { FirstName = "Bob", LastName = "Marley", BirthDate = new DateOnly(1945, 2, 6) },
                    new Patient { FirstName = "Alice", LastName = "Wonderland", BirthDate = new DateOnly(2010, 12, 12) },
                    new Patient { FirstName = "Lucky", LastName = "Luke", BirthDate = new DateOnly(1960, 1, 1) }
                };
                context.Patients.AddRange(patients);
                context.SaveChanges();
            }


            
            if (!context.Consultations.Any())
            {
                var doctors = context.Doctors.ToList();
                var patients = context.Patients.ToList();
                var meds = context.Medicaments.ToList();

                var drHouse = doctors.First(d => d.LastName == "House");
                var drGrey = doctors.First(d => d.LastName == "Grey");
                var p1 = patients[0];
                var p2 = patients[1];
                var p3 = patients[2];

                var today = DateOnly.FromDateTime(DateTime.Today);
                var time9 = new TimeOnly(9, 0);
                var time14 = new TimeOnly(14, 0);

                var consultations = new List<Consultation>
                {
                    new Consultation { Date = today, Hour = time9, Reason = "Migraine", Doctor = drHouse, Patient = p1 },
                    new Consultation { Date = today, Hour = time14, Reason = "Suivi", Doctor = drHouse, Patient = p2 },
                    new Consultation { Date = today.AddDays(-1), Hour = time9, Reason = "Douleur", Doctor = drGrey, Patient = p1 },
                    new Consultation { Date = today.AddDays(-2), Hour = time14, Reason = "Bilan", Doctor = drGrey, Patient = p3 },
                    new Consultation { Date = today.AddDays(-3), Hour = time9, Reason = "Checkup", Doctor = doctors[4], Patient = p1 },
                    new Consultation { Date = today.AddDays(-4), Hour = time14, Reason = "Vision", Doctor = doctors[5], Patient = p3 },
                    new Consultation { Date = today.AddDays(-5), Hour = time9, Reason = "Urgence", Doctor = doctors[6], Patient = p2 },
                    new Consultation { Date = today.AddDays(-6), Hour = time14, Reason = "Routine", Doctor = drHouse, Patient = p3 },
                    new Consultation { Date = today.AddDays(-7), Hour = time9, Reason = "Suivi", Doctor = drGrey, Patient = p2 },
                    new Consultation { Date = today.AddDays(-8), Hour = time14, Reason = "Consult", Doctor = doctors[1], Patient = p1 }
                };
                context.Consultations.AddRange(consultations);
                context.SaveChanges();

                var paracetamol = meds.FirstOrDefault(m => m.Name == "Paracetamol");
                var amoxicilline = meds.FirstOrDefault(m => m.Name == "Amoxicilline");
                var ibuprofen = meds.FirstOrDefault(m => m.Name == "Ibuprofene");

                if (paracetamol != null)
                {
                    var prescriptions = new List<Prescription>
                    {
                        new Prescription
                        {
                            Date = today,
                            Doctor = drHouse,
                            Patient = p1,
                            Consultation = consultations[0],
                            Lines = new List<PrescriptionLine>
                            {
                                new PrescriptionLine 
                                { 
                                    Medicament = paracetamol, 
                                    Quantity = 2, 
                                    Dosage = "1g", 
                                    Frequency = "3x/jour", 
                                    Duration = "5 jours", 
                                    Instructions = "Si douleur" 
                                }
                            }
                        },
                        new Prescription
                        {
                            Date = today,
                            Doctor = drHouse,
                            Patient = p2,
                            Consultation = consultations[1],
                            Lines = new List<PrescriptionLine>
                            {
                                new PrescriptionLine 
                                { 
                                    Medicament = ibuprofen ?? paracetamol, 
                                    Quantity = 1, 
                                    Dosage = "400mg", 
                                    Frequency = "Matin et soir", 
                                    Duration = "3 jours", 
                                    Instructions = "Pendant repas" 
                                }
                            }
                        }
                    };
                    context.Prescriptions.AddRange(prescriptions);
                    context.SaveChanges();
                }
            }
        }
    }
}
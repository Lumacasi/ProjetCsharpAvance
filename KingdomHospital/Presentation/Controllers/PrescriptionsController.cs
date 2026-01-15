using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Controllers
{
    [ApiController]
    [Route("api/ordonnances")] // Route imposée par la consigne
    public class PrescriptionsController : ControllerBase
    {
        private readonly KingdomHospitalContext _context;
        private readonly PrescriptionMapper _mapper;

        public PrescriptionsController(KingdomHospitalContext context, PrescriptionMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ordonnances (avec filtres)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptions(
            [FromQuery] int? doctorId, 
            [FromQuery] int? patientId, 
            [FromQuery] DateOnly? from, 
            [FromQuery] DateOnly? to)
        {
            var query = _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Lines).ThenInclude(l => l.Medicament)
                .AsQueryable();

            if (doctorId.HasValue) query = query.Where(p => p.DoctorId == doctorId);
            if (patientId.HasValue) query = query.Where(p => p.PatientId == patientId);
            if (from.HasValue) query = query.Where(p => p.Date >= from.Value);
            if (to.HasValue) query = query.Where(p => p.Date <= to.Value);

            var list = await query.ToListAsync();
            // Transformation en DTO
            return Ok(list.Select(p => _mapper.ToDto(p)));
        }

        // GET: api/ordonnances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDto>> GetPrescription(int id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Lines).ThenInclude(l => l.Medicament)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null) return NotFound();

            return Ok(_mapper.ToDto(prescription));
        }

        // POST: api/ordonnances
        [HttpPost]
        public async Task<ActionResult<PrescriptionDto>> CreatePrescription(CreatePrescriptionDto dto)
        {
            // Vérif FK
            if (!await _context.Doctors.AnyAsync(d => d.Id == dto.DoctorId) ||
                !await _context.Patients.AnyAsync(p => p.Id == dto.PatientId))
            {
                return BadRequest("Médecin ou Patient introuvable.");
            }

            // Transformation DTO -> Entité
            var prescription = _mapper.ToEntity(dto);

            // On ajoute aussi les lignes manuellement si Mapperly ne le fait pas automatiquement (dépend de ta config)
            // Mais généralement, si le DTO contient la liste 'Lines', Mapperly essaiera de mapper.
            // Assure-toi que ton Mapper Prescription a bien une méthode pour mapper les lignes imbriquées,
            // ou ajoute-les ici :
            if (dto.Lines != null && dto.Lines.Any())
            {
                foreach(var lineDto in dto.Lines)
                {
                    // Si Mapperly n'a pas mappé la liste automatiquement, on peut le faire ici
                    // prescription.Lines.Add(_mapper.LineToEntity(lineDto)); 
                    // (Mais normalement .ToEntity(dto) gère la liste si les noms matchent)
                }
            }

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            // Rechargement pour l'affichage complet
            var loaded = await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Lines).ThenInclude(l => l.Medicament)
                .FirstAsync(p => p.Id == prescription.Id);

            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.Id }, _mapper.ToDto(loaded));
        }

        // PUT: api/ordonnances/5 (Mise à jour simple, sans les lignes)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(int id, CreatePrescriptionDto dto)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null) return NotFound();

            // Ici on ne met à jour que l'en-tête (Date, Notes, FKs)
            // Pour les lignes, on utilise les endpoints dédiés (plus sûr)
            prescription.Date = dto.Date;
            prescription.Notes = dto.Notes;
            prescription.DoctorId = dto.DoctorId;
            prescription.PatientId = dto.PatientId;
            prescription.ConsultationId = dto.ConsultationId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ordonnances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null) return NotFound();

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: Rattacher consultation
        [HttpPut("{id}/consultation/{consultationId}")]
        public async Task<IActionResult> AttachConsultation(int id, int consultationId)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null) return NotFound("Ordonnance introuvable");

            if (!await _context.Consultations.AnyAsync(c => c.Id == consultationId)) 
                return BadRequest("Consultation introuvable");

            prescription.ConsultationId = consultationId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: Détacher consultation
        [HttpDelete("{id}/consultation")]
        public async Task<IActionResult> DetachConsultation(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null) return NotFound();

            prescription.ConsultationId = null;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- GESTION DES LIGNES (Via DTOs) ---

        [HttpGet("{id}/lignes")]
        public async Task<ActionResult<IEnumerable<PrescriptionLineDto>>> GetLines(int id)
        {
            if (!await _context.Prescriptions.AnyAsync(p => p.Id == id)) return NotFound();
            
            var lines = await _context.PrescriptionLines
                .Where(l => l.PrescriptionId == id)
                .Include(l => l.Medicament)
                .ToListAsync();

            return Ok(lines.Select(l => _mapper.LineToDto(l)));
        }

        [HttpPost("{id}/lignes")]
        public async Task<ActionResult> AddLines(int id, List<CreatePrescriptionLineDto> linesDto)
        {
            if (!await _context.Prescriptions.AnyAsync(p => p.Id == id)) return NotFound("Ordonnance introuvable");

            var newLines = new List<PrescriptionLineDto>();

            foreach(var dto in linesDto)
            {
                var line = _mapper.LineToEntity(dto);
                line.PrescriptionId = id; // Force l'ID parent
                _context.PrescriptionLines.Add(line);
                
                // Petite astuce pour le retour : on devra recharger le médicament pour avoir le nom
                // Mais pour l'instant on sauvegarde juste.
            }
            await _context.SaveChangesAsync();
            
            return Ok("Lignes ajoutées");
        }

        [HttpPut("{id}/lignes/{ligneId}")]
        public async Task<IActionResult> UpdateLine(int id, int ligneId, CreatePrescriptionLineDto dto)
        {
            var existingLine = await _context.PrescriptionLines
                .FirstOrDefaultAsync(l => l.Id == ligneId && l.PrescriptionId == id);
            
            if (existingLine == null) return NotFound();

            // Mise à jour manuelle ou via mapper si configuré (UpdateEntity)
            existingLine.MedicamentId = dto.MedicamentId;
            existingLine.Dosage = dto.Dosage;
            existingLine.Duration = dto.Duration;
            existingLine.Frequency = dto.Frequency;
            existingLine.Quantity = dto.Quantity;
            existingLine.Instructions = dto.Instructions;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}/lignes/{ligneId}")]
        public async Task<IActionResult> DeleteLine(int id, int ligneId)
        {
             var line = await _context.PrescriptionLines
                 .FirstOrDefaultAsync(l => l.Id == ligneId && l.PrescriptionId == id);
             
             if (line == null) return NotFound();
             
             _context.PrescriptionLines.Remove(line);
             await _context.SaveChangesAsync();
             return NoContent();
        }
    }
}
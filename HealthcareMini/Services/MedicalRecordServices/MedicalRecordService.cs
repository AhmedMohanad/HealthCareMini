// Services/MedicalRecordServices/MedicalRecordService.cs
using HealthcareMini.Data;
using HealthcareMini.DTOs.MedicalRecord;
using HealthcareMini.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.MedicalRecordServices
{
    public class MedicalRecordService
    {
        private readonly HealthcareDbContext _context;

        public MedicalRecordService(HealthcareDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(m => m.Doctor)
                .Include(m => m.Patient)
                .Include(m => m.HealthCareCenter)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _context.MedicalRecords
                .Include(m => m.Doctor)
                .Include(m => m.HealthCareCenter)
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.HealthCareCenter)
                .Where(m => m.DoctorId == doctorId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<MedicalRecord> CreateAsync(MedicalRecordRequestDTO dto)
        {
            var record = new MedicalRecord
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                HealthCareCenterId = dto.HealthCareCenterId,
                Description = dto.Description,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.MedicalRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<MedicalRecord?> UpdateAsync(int id, MedicalRecordRequestDTO dto)
        {
            var record = await _context.MedicalRecords.FindAsync(id);
            if (record == null) return null;

            record.Description = dto.Description ?? record.Description;
            record.Notes = dto.Notes ?? record.Notes;

            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _context.MedicalRecords.FindAsync(id);
            if (record == null) return false;
            _context.MedicalRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
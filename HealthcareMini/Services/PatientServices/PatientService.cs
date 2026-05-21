// Services/PatientServices/PatientService.cs
using HealthcareMini.Data;
using HealthcareMini.DTOs.Patient;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.PatientServices
{
    public class PatientService
    {
        private readonly HealthcareDbContext _context;

        public PatientService(HealthcareDbContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Include(p => p.ContactDetails)
                .Include(p => p.AddressDetails)
                .Include(p => p.MedicalRecords)
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patient?> GetByEmailAsync(string email)
        {
            return await _context.Patients
                .Include(p => p.ContactDetails)
                .Include(p => p.AddressDetails)
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.ContactDetails)
                .Include(p => p.AddressDetails)
                .ToListAsync();
        }

        public async Task<Patient> CreateAsync(PatientRequestDTO dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                DateOfBirth = dto.DateOfBirth,
                ContactDetails = dto.ContactDetails,
                AddressDetails = dto.AddressDetails,
                Role = UserRole.Patient
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient?> UpdateAsync(int id, PatientRequestDTO dto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return null;

            patient.FirstName = dto.FirstName ?? patient.FirstName;
            patient.LastName = dto.LastName ?? patient.LastName;
            patient.Email = dto.Email ?? patient.Email;
            patient.DateOfBirth = dto.DateOfBirth;
            patient.ContactDetails = dto.ContactDetails ?? patient.ContactDetails;
            patient.AddressDetails = dto.AddressDetails ?? patient.AddressDetails;

            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.HealthCareCenter)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetPatientMedicalRecordsAsync(int patientId)
        {
            return await _context.MedicalRecords
                .Include(m => m.Doctor)
                .Include(m => m.HealthCareCenter)
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}
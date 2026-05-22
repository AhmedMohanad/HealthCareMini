// Services/DoctorServices/DoctorService.cs
using HealthcareMini.Data;
using HealthcareMini.DTOs.Doctor;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Services.PasswordServices;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.DoctorServices
{
    public class DoctorService : IDoctorService
    {
        private readonly HealthcareDbContext _context;
        private readonly IPasswordService _passwordService;

        public DoctorService(HealthcareDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.ContactDetails)
                .Include(d => d.AddressDetails)
                .Include(d => d.HealthCareCenters)
                
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor?> GetByEmailAsync(string email)
        {
            return await _context.Doctors
                .Include(d => d.HealthCareCenters)
                .FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.ContactDetails)
                .Include(d => d.HealthCareCenters)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsByCenterAsync(int centerId)
        {
            var center = await _context.HealthCareCenters
                .Include(c => c.Doctors)
                .FirstOrDefaultAsync(c => c.Id == centerId);
            return center?.Doctors ?? new List<Doctor>();
        }

        public async Task<Doctor> CreateAsync(DoctorRequestDTO dto)
        {
            var doctor = new Doctor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                DateOfBirth = dto.DateOfBirth,
                ContactDetails = dto.ContactDetails,
                AddressDetails = dto.AddressDetails,
                Salary = dto.Salary,
                Specialization = dto.Specialization,
                IsActive = true,
                Role = UserRole.Doctor
            };

            if (dto.HealthCareCenterIds?.Any() == true)
            {
                var centers = await _context.HealthCareCenters
                    .Where(c => dto.HealthCareCenterIds.Contains(c.Id))
                    .ToListAsync();
                doctor.HealthCareCenters = centers;
            }

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor?> UpdateAsync(int id, DoctorRequestDTO dto)
        {
            var doctor = await _context.Doctors
                .Include(d => d.HealthCareCenters)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null) return null;

            doctor.FirstName = dto.FirstName ?? doctor.FirstName;
            doctor.LastName = dto.LastName ?? doctor.LastName;
            doctor.Email = dto.Email ?? doctor.Email;
            doctor.Salary = dto.Salary;
            doctor.Specialization = dto.Specialization ?? doctor.Specialization;
            doctor.IsActive = dto.IsActive;

            if (dto.HealthCareCenterIds?.Any() == true)
            {
                var centers = await _context.HealthCareCenters
                    .Where(c => dto.HealthCareCenterIds.Contains(c.Id))
                    .ToListAsync();
                doctor.HealthCareCenters = centers;
            }

            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return false;
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateTime? date = null)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId);

            if (date.HasValue)
                query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);

            return await query.OrderBy(a => a.AppointmentDate).ToListAsync();
        }
    }
}
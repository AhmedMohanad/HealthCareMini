using HealthcareMini.Data;
using HealthcareMini.DTOs.Doctor;
using HealthcareMini.DTOs.Receptionist;
using HealthcareMini.DTOs.Staff;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Services.PasswordServices;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    public class HealthCareCenterEmployeeService : HealthCareCenterBaseService, IHealthCareCenterEmployeeService
    {

        private readonly IPasswordService _passwordService;
        public HealthCareCenterEmployeeService(HealthcareDbContext context, IPasswordService passwordService) : base(context, passwordService) { }

        // =====================================================================
        // DOCTOR MANAGEMENT
        // =====================================================================

        // Called from the controller with a DTO (preferred path).
        public async Task<Doctor?> AddDoctorAsync(int centerId, DoctorRequestDTO dto)
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
            return await AddDoctorAsync(centerId, doctor);
        }

        // Core implementation — also used internally and by the facade.
        public async Task<Doctor?> AddDoctorAsync(int centerId, Doctor doctor)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Doctors)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return null;

                if (center.Doctors.Any(d => d.Email == doctor.Email))
                    throw new InvalidOperationException("A doctor with this email already works at this center.");

                var existing = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Email == doctor.Email);

                if (existing != null)
                {
                    center.Doctors.Add(existing);
                    await _context.SaveChangesAsync();
                    return existing;
                }

                center.Doctors.Add(doctor);
                await _context.SaveChangesAsync();
                return doctor;
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                throw new Exception("An error occurred while adding the doctor.", ex);
            }
        }

        public async Task<IEnumerable<Doctor>> AddDoctorsAsync(int centerId, IEnumerable<Doctor> doctors)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Doctors)
                    .FirstOrDefaultAsync(h => h.Id == centerId)
                    ?? throw new Exception($"Health care center with ID {centerId} not found.");

                var added = new List<Doctor>();

                foreach (var doctor in doctors)
                {
                    if (center.Doctors.Any(d => d.Email == doctor.Email))
                        continue;

                    var existing = await _context.Doctors
                        .FirstOrDefaultAsync(d => d.Email == doctor.Email);

                    var target = existing ?? doctor;
                    center.Doctors.Add(target);
                    added.Add(target);
                }

                await _context.SaveChangesAsync();
                return added;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding doctors.", ex);
            }
        }

        public async Task<bool> RemoveDoctorFromCenterAsync(int centerId, int doctorId)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Doctors)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null) return false;

                var doctor = center.Doctors.FirstOrDefault(d => d.Id == doctorId);
                if (doctor == null) return false;

                center.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the doctor from center.", ex);
            }
        }

        // =====================================================================
        // RECEPTIONIST MANAGEMENT
        // =====================================================================

        public async Task<Receptionist?> AddReceptionistAsync(int centerId, ReceptionistRequestDTO dto)
        {
            var receptionist = new Receptionist
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                DateOfBirth = dto.DateOfBirth,
                ContactDetails = dto.ContactDetails,
                AddressDetails = dto.AddressDetails,
                Salary = dto.Salary,
                Role = UserRole.Receptionist
            };
            return await AddReceptionistAsync(centerId, receptionist);
        }

        public async Task<Receptionist?> AddReceptionistAsync(int centerId, Receptionist receptionist)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Receptionists)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null) return null;

                if (center.Receptionists.Any(r => r.Email == receptionist.Email))
                    throw new InvalidOperationException("A receptionist with this email already works at this center.");

                var existing = await _context.Receptionists
                    .FirstOrDefaultAsync(r => r.Email == receptionist.Email);

                if (existing != null)
                {
                    center.Receptionists.Add(existing);
                    await _context.SaveChangesAsync();
                    return existing;
                }

                center.Receptionists.Add(receptionist);
                await _context.SaveChangesAsync();
                return receptionist;
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                throw new Exception("An error occurred while adding the receptionist.", ex);
            }
        }

        public async Task<IEnumerable<Receptionist>> AddReceptionistsAsync(int centerId, IEnumerable<Receptionist> receptionists)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Receptionists)
                    .FirstOrDefaultAsync(h => h.Id == centerId)
                    ?? throw new Exception($"Health care center with ID {centerId} not found.");

                var added = new List<Receptionist>();

                foreach (var r in receptionists)
                {
                    if (center.Receptionists.Any(x => x.Email == r.Email))
                        continue;

                    var existing = await _context.Receptionists
                        .FirstOrDefaultAsync(x => x.Email == r.Email);

                    var target = existing ?? r;
                    center.Receptionists.Add(target);
                    added.Add(target);
                }

                await _context.SaveChangesAsync();
                return added;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding receptionists.", ex);
            }
        }

        public async Task<bool> RemoveReceptionistFromCenterAsync(int centerId, int receptionistId)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Receptionists)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null) return false;

                var r = center.Receptionists.FirstOrDefault(x => x.Id == receptionistId);
                if (r == null) return false;

                center.Receptionists.Remove(r);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the receptionist from center.", ex);
            }
        }

        // =====================================================================
        // STAFF MANAGEMENT
        // =====================================================================

        public async Task<Staff?> AddStaffAsync(int centerId, StaffRequestDTO dto)
        {
            var staff = new Staff
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                DateOfBirth = dto.DateOfBirth,
                ContactDetails = dto.ContactDetails,
                AddressDetails = dto.AddressDetails,
                Salary = dto.Salary,
                JobTitle = dto.JobTitle,
                Role = UserRole.Staff
            };
            return await AddStaffAsync(centerId, staff);
        }

        public async Task<Staff?> AddStaffAsync(int centerId, Staff staff)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Staff)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null) return null;

                if (center.Staff.Any(s => s.Email == staff.Email))
                    throw new InvalidOperationException("A staff member with this email already works at this center.");

                var existing = await _context.Staff
                    .FirstOrDefaultAsync(s => s.Email == staff.Email);

                if (existing != null)
                {
                    center.Staff.Add(existing);
                    await _context.SaveChangesAsync();
                    return existing;
                }

                center.Staff.Add(staff);
                await _context.SaveChangesAsync();
                return staff;
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                throw new Exception("An error occurred while adding the staff member.", ex);
            }
        }

        public async Task<IEnumerable<Staff>> AddStaffAsync(int centerId, IEnumerable<Staff> staffList)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Staff)
                    .FirstOrDefaultAsync(h => h.Id == centerId)
                    ?? throw new Exception($"Health care center with ID {centerId} not found.");

                var added = new List<Staff>();

                foreach (var s in staffList)
                {
                    if (center.Staff.Any(x => x.Email == s.Email))
                        continue;

                    var existing = await _context.Staff
                        .FirstOrDefaultAsync(x => x.Email == s.Email);

                    var target = existing ?? s;
                    center.Staff.Add(target);
                    added.Add(target);
                }

                await _context.SaveChangesAsync();
                return added;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding staff members.", ex);
            }
        }

        public async Task<bool> RemoveStaffFromCenterAsync(int centerId, int staffId)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Staff)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null) return false;

                var s = center.Staff.FirstOrDefault(x => x.Id == staffId);
                if (s == null) return false;

                center.Staff.Remove(s);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the staff member from center.", ex);
            }
        }
    }
}
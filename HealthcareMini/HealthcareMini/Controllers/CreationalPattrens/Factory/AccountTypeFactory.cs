using HealthcareMini.Data;
using HealthcareMini.DTOs;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Objects;
using HealthcareMini.Services;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Controllers.CreationalPattrens.Factory
{
    public class AccountTypeFactory
    {
        private readonly HealthcareDbContext _context;
        private HealthCareCenterServices _centerService;

        public AccountTypeFactory(HealthcareDbContext context)
        {
            _context = context;
            _centerService = new HealthCareCenterServices(context);
        }

        public async Task CreateUserAsync(UserDto dto)
        {
            // TODO: Replace with your actual BCrypt / Password Hashing logic
            string hashedPassword = dto.Password + "_HASHED";

            // Fallback object initialization to prevent null entity errors
            var contact = dto.ContactDetails ?? new ContactDetails();
            var address = dto.AddressDetails ?? new AddressDetails();

            switch (dto.Role)
            {
                case UserRole.Admin:
                    var admin = new Admin
                    {
                        Email = dto.Email,
                        PasswordHash = hashedPassword,
                        FirstName = dto.FirstName ?? string.Empty,
                        LastName = dto.LastName ?? string.Empty,
                        DateOfBirth = dto.DateOfBirth ?? DateTime.MinValue,
                        ContactDetails = contact,
                        AddressDetails = address
                    };
                    _context.Admins.Add(admin);
                    break;

                case UserRole.Doctor:
                    var doctor = new Doctor
                    {
                        Email = dto.Email,
                        PasswordHash = hashedPassword,
                        FirstName = dto.FirstName ?? string.Empty,
                        LastName = dto.LastName ?? string.Empty,
                        DateOfBirth = dto.DateOfBirth ?? DateTime.MinValue,
                        ContactDetails = contact,
                        AddressDetails = address,
                        Salary = dto.Salary ?? 0.0,
                        Specialization = dto.Specialization ?? "General Medicine"
                    };

                    // Link to existing centers if IDs are provided
                    if (dto.HealthCareCenterIds != null)
                    {
                        doctor.HealthCareCenters = await _context.HealthCareCenters
                            .Where(c => dto.HealthCareCenterIds.Contains(c.Id)).ToListAsync();
                    }
                    _context.Doctors.Add(doctor);
                    break;

                case UserRole.Staff:
                    var staff = new Staff
                    {
                        Email = dto.Email,
                        PasswordHash = hashedPassword,
                        FirstName = dto.FirstName ?? string.Empty,
                        LastName = dto.LastName ?? string.Empty,
                        DateOfBirth = dto.DateOfBirth ?? DateTime.MinValue,
                        ContactDetails = contact,
                        AddressDetails = address,
                        Salary = dto.Salary ?? 0.0,
                        JobTitle = dto.JobTitle ?? "General Staff"
                    };
                    if (dto.HealthCareCenterIds != null)
                    {
                        staff.HealthCareCenters = await _context.HealthCareCenters
                            .Where(c => dto.HealthCareCenterIds.Contains(c.Id)).ToListAsync();
                    }
                    _context.Staff.Add(staff);
                    break;

                case UserRole.HealthCareCenter:
                    // Notice how this does NOT inherit from User, but the factory handles it cleanly!
                    var center = new HealthCareCenter
                    {
                        Email = dto.Email,
                        PasswordHash = hashedPassword,
                        Name = dto.Name ?? "Unnamed Center",
                        IsActive = true,
                        ContactDetails = contact,
                        AddressDetails = address
                    };
                   await _centerService.CreateAsync(center);
                    break;

                case UserRole.Patient:
                    var patient = new Patient
                    {
                        Email = dto.Email,
                        PasswordHash = hashedPassword,
                        FirstName = dto.FirstName ?? string.Empty,
                        LastName = dto.LastName ?? string.Empty,
                        DateOfBirth = dto.DateOfBirth ?? DateTime.MinValue,
                        ContactDetails = contact,
                        AddressDetails = address,
                        MedicalRecords = [] // Starts empty
                    };
                    _context.Patients.Add(patient);
                    break;

                default:
                    throw new ArgumentException("Unsupported or invalid user role.");
            }

            // Save the entity changes safely to your database
            await _context.SaveChangesAsync();
        }
    }
}
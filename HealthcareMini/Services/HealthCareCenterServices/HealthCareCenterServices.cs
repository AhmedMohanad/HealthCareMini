using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Interfaces;
using HealthcareMini.Models.Objects;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    public class HealthCareCenterServices : IHealthCareCenterServices
    {
        private readonly HealthcareDbContext _context;

        public HealthCareCenterServices(HealthcareDbContext context)
        {
            _context = context;
        }

        public async Task<ResponsCenter> CreateAsync(CreateCenterDTO healthcareCenter)
        {
            HealthCareCenter newHealthCareCenter;

            try
            {
                newHealthCareCenter = new HealthCareCenter
                {
                    Name = healthcareCenter.Name,
                    Email = healthcareCenter.Email,
                    PasswordHash = healthcareCenter.PasswordHash,
                    IsActive = false,
                    Role = UserRole.HealthCareCenter,
                    ContactDetails = new ContactDetails
                    {
                        PhoneNumbers = healthcareCenter.ContactDetails?.PhoneNumbers ?? new List<string>()
                    },
                    AddressDetails = new AddressDetails
                    {
                        Street = healthcareCenter.AddressDetails?.Street ?? string.Empty,
                        City = healthcareCenter.AddressDetails?.City ?? string.Empty,
                        Province = healthcareCenter.AddressDetails?.Province ?? Province.Baghdad,
                        ZipCode = healthcareCenter.AddressDetails?.ZipCode ?? string.Empty
                    },
                    Doctors = healthcareCenter.Doctors ?? new List<Doctor>(),
                    Receptionists = healthcareCenter.Receptionists ?? new List<Receptionist>(),
                    Staff = healthcareCenter.Staff ?? new List<Staff>(),
                    Appointments = healthcareCenter.Appointments ?? new List<Appointment>()
                };
                _context.HealthCareCenters.Add(newHealthCareCenter);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the health care center.", ex);
            }

            // Return DTO
            return new ResponsCenter
            {
                Email = newHealthCareCenter.Email,
                Name = newHealthCareCenter.Name,
                ContactDetails = newHealthCareCenter.ContactDetails,
                AddressDetails = newHealthCareCenter.AddressDetails,
                Doctors = newHealthCareCenter.Doctors,
                Receptionists = newHealthCareCenter.Receptionists,
                Staff = newHealthCareCenter.Staff,
                Appointments = newHealthCareCenter.Appointments
            };
        }

        public async Task<ResponsCenter?> EditAsync(int id, EditCenterDTO dto)
        {
            var center = await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (center == null)
                return null;

            center.Name = dto.Name ?? center.Name;
            center.Email = dto.Email ?? center.Email;
            center.PasswordHash = dto.PasswordHash ?? center.PasswordHash;

            if (dto.ContactDetails != null)
            {
                center.ContactDetails ??= new ContactDetails();
                center.ContactDetails.PhoneNumbers = dto.ContactDetails.PhoneNumbers ?? center.ContactDetails.PhoneNumbers;
            }

            if (dto.AddressDetails != null)
            {
                center.AddressDetails ??= new AddressDetails();
                center.AddressDetails.Street = dto.AddressDetails.Street ?? center.AddressDetails.Street;
                center.AddressDetails.City = dto.AddressDetails.City ?? center.AddressDetails.City;
                center.AddressDetails.Province = dto.AddressDetails.Province ?? center.AddressDetails.Province;
                center.AddressDetails.ZipCode = dto.AddressDetails.ZipCode ?? center.AddressDetails.ZipCode;
            }

            await _context.SaveChangesAsync();

            // Return DTO
            return new ResponsCenter
            {
                Id = center.Id,
                Email = center.Email,
                Name = center.Name,
                ContactDetails = center.ContactDetails,
                AddressDetails = center.AddressDetails,
                Doctors = center.Doctors,
                Receptionists = center.Receptionists,
                Staff = center.Staff,
                Appointments = center.Appointments
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var center = await _context.HealthCareCenters.FindAsync(id);
            if (center == null)
                return false;
            _context.HealthCareCenters.Remove(center);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ResponsCenter?> GetByIdAsync(int id)
        {
            var center = await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .Include(h => h.Doctors)
                .Include(h => h.Receptionists)
                .Include(h => h.Staff)
                .Include(h => h.Appointments)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (center == null)
                return null;

            // Return DTO
            return new ResponsCenter
            {
                Id = center.Id,
                Email = center.Email,
                Name = center.Name,
                ContactDetails = center.ContactDetails,
                AddressDetails = center.AddressDetails,
                Doctors = center.Doctors,
                Receptionists = center.Receptionists,
                Staff = center.Staff,
                Appointments = center.Appointments
            };
        }

        public async Task<ResponsCenter?> GetByEmailAsync(string email)
        {
            var center = await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .Include(h => h.Doctors)
                .Include(h => h.Receptionists)
                .Include(h => h.Staff)
                .Include(h => h.Appointments)
                .FirstOrDefaultAsync(h => h.Email == email);

            if (center == null)
                return null;

            // Return DTO
            return new ResponsCenter
            {
                Id = center.Id,
                Email = center.Email,
                Name = center.Name,
                ContactDetails = center.ContactDetails,
                AddressDetails = center.AddressDetails,
                Doctors = center.Doctors,
                Receptionists = center.Receptionists,
                Staff = center.Staff,
                Appointments = center.Appointments
            };
        }

        public async Task<IEnumerable<LimitedResponsCenter>> GetAllAsync()
        {
            var centers = await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .ToListAsync();

            // Return limited DTOs (without collections)
            return centers.Select(center => new LimitedResponsCenter
            {
                Id = center.Id,
                Email = center.Email,
                Name = center.Name,
                ContactDetails = center.ContactDetails,
                AddressDetails = center.AddressDetails
            }).ToList();
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var center = await _context.HealthCareCenters.FindAsync(id);
            if (center == null)
                return false;
            center.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var center = await _context.HealthCareCenters.FindAsync(id);
            if (center == null)
                return false;
            center.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LimitedResponsCenter?> GetByNameAsync(string name)
        {
            var center = await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .FirstOrDefaultAsync(h => h.Name == name);

            if (center == null)
                return null;

            // Return limited DTO
            return new LimitedResponsCenter
            {
                Id = center.Id,
                Email = center.Email,
                Name = center.Name,
                ContactDetails = center.ContactDetails,
                AddressDetails = center.AddressDetails
            };
        }

        public async Task<IEnumerable<IEmployee>> GetEmployeesAsync(int centerId)
        {
            var center = await _context.HealthCareCenters
                .Include(h => h.Doctors)
                .Include(h => h.Receptionists)
                .Include(h => h.Staff)
                .FirstOrDefaultAsync(h => h.Id == centerId);

            if (center == null)
                return Enumerable.Empty<IEmployee>();

            var employees = new List<IEmployee>();
            employees.AddRange(center.Doctors);
            employees.AddRange(center.Receptionists);
            employees.AddRange(center.Staff);
            return employees;
        }

        public async Task<IEnumerable<LimitedResponsCenter>> GetLimitedAsync()
        {
            var centers = await _context.HealthCareCenters
                .Select(h => new LimitedResponsCenter
                {
                    Id = h.Id,
                    Email = h.Email,
                    Name = h.Name,
                    ContactDetails = new ContactDetails
                    {
                        PhoneNumbers = h.ContactDetails.PhoneNumbers
                    },
                    AddressDetails = new AddressDetails
                    {
                        Street = h.AddressDetails.Street,
                        City = h.AddressDetails.City,
                        Province = h.AddressDetails.Province,
                        ZipCode = h.AddressDetails.ZipCode
                    }
                })
                .ToListAsync();

            return centers;
        }

        public async Task<bool> DeleteByEmailAsync(string email)
        {
            var center = await _context.HealthCareCenters.FirstOrDefaultAsync(h => h.Email == email);
            if (center == null)
                return false;
            _context.HealthCareCenters.Remove(center);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
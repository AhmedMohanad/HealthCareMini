using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Interfaces;
using HealthcareMini.Models.Objects;
using Microsoft.AspNetCore.Identity.UI.Services;
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

        public async Task<HealthCareCenter> CreateAsync(CreateCenterDTO healthcareCenter)
        {

            // create a new instance of HealthCareCenter and copy properties from the input healthcareCenter

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

                    // any thing else can be null or empty or default value
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
                // Handle exceptions (e.g., log the error, rethrow, or return a specific result)
                throw new Exception("An error occurred while creating the health care center.", ex);
            }



            return newHealthCareCenter;
        }


        public async Task<HealthCareCenter?> EditAsync(int id, EditCenterDTO dto)
        {
            var center = await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (center == null)
                return null;

            // Update fields directly (null ignores if not provided)
            center.Name = dto.Name ?? center.Name;
            center.Email = dto.Email ?? center.Email;
            center.PasswordHash = dto.PasswordHash ?? center.PasswordHash;

            // Update ContactDetails
            if (dto.ContactDetails != null)
            {
                center.ContactDetails ??= new ContactDetails();
                center.ContactDetails.PhoneNumbers = dto.ContactDetails.PhoneNumbers ?? center.ContactDetails.PhoneNumbers;

            }

            // Update AddressDetails
            if (dto.AddressDetails != null)
            {
                center.AddressDetails ??= new AddressDetails();
                center.AddressDetails.Street = dto.AddressDetails.Street ?? center.AddressDetails.Street;
                center.AddressDetails.City = dto.AddressDetails.City ?? center.AddressDetails.City;
                center.AddressDetails.Province = dto.AddressDetails.Province ?? center.AddressDetails.Province;
                center.AddressDetails.ZipCode = dto.AddressDetails.ZipCode ?? center.AddressDetails.ZipCode;
            }

            await _context.SaveChangesAsync();
            return center;
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
        
        public async Task<HealthCareCenter?> GetByIdAsync(int id)
        {
            return await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<HealthCareCenter?> GetByEmailAsync(string email)
        {
            return await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .FirstOrDefaultAsync(h => h.Email == email);
        }


        public async Task<IEnumerable<HealthCareCenter>> GetAllAsync()
        {
            return await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .ToListAsync();
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
        public async Task<HealthCareCenter?> GetByNameAsync(string name)
        {
            return await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .FirstOrDefaultAsync(h => h.Name == name);
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

        //this method return only general information about the health care center without the related entities like doctors and receptionists and staff and appointments
        public async Task<IEnumerable<HealthCareCenter>> GetLimitedAsync()
        {
            var centers = await _context.HealthCareCenters
                .Select(h => new HealthCareCenter
                {
                    
                    Name = h.Name,
                    Email = h.Email,
                    IsActive = h.IsActive,
                    
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

        //this method is to delete the center by email
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
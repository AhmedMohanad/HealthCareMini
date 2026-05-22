using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Interfaces;
using HealthcareMini.Models.Objects;
using HealthcareMini.Services.PasswordServices;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    // Service for query and read operations
    public class HealthCareCenterQueryService : HealthCareCenterBaseService , IHealthCareCenterQueryService
    {
        private readonly IPasswordService _passwordService; 
        public HealthCareCenterQueryService(HealthcareDbContext context, IPasswordService passwordService) : base(context, passwordService)
        {
            _passwordService = passwordService;
        }

        // Get health care center by ID with all related data
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

        // Get health care center by email with all related data
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

        // Get all health care centers (limited data, no collections)
        public async Task<IEnumerable<LimitedResponsCenter>> GetAllAsync()
        {
            var centers = await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .ToListAsync();

            return centers.Select(center => new LimitedResponsCenter
            {
                Id = center.Id,
                Email = center.Email,
                Name = center.Name,
                ContactDetails = center.ContactDetails,
                AddressDetails = center.AddressDetails
            }).ToList();
        }

        // Get limited health care center data for public view
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

        // Get health care center by name (limited data)
        public async Task<LimitedResponsCenter?> GetByNameAsync(string name)
        {
            var center = await _context.HealthCareCenters
                .Include(h => h.ContactDetails)
                .Include(h => h.AddressDetails)
                .FirstOrDefaultAsync(h => h.Name == name);

            if (center == null)
                return null;

            return new LimitedResponsCenter
            {
                Id = center.Id,
                Email = center.Email,
                Name = center.Name,
                ContactDetails = center.ContactDetails,
                AddressDetails = center.AddressDetails
            };
        }

        // Get all employees (doctors, receptionists, staff) for a specific center
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
    }
}
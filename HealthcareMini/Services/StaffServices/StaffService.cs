// Services/StaffServices/StaffService.cs
using HealthcareMini.Data;
using HealthcareMini.DTOs.Staff;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Services.PasswordServices;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.StaffServices
{
    public class StaffService : IStaffService
    {
        private readonly HealthcareDbContext _context;
        private readonly IPasswordService _passwordService;

        public StaffService(HealthcareDbContext context, IPasswordService passwordService)
        {
            _context = context;
        }

        public async Task<Staff?> GetByIdAsync(int id)
        {
            return await _context.Staff
                .Include(s => s.ContactDetails)
                .Include(s => s.AddressDetails)
                .Include(s => s.HealthCareCenters)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            return await _context.Staff
                .Include(s => s.ContactDetails)
                .Include(s => s.HealthCareCenters)
                .ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetStaffByCenterAsync(int centerId)
        {
            var center = await _context.HealthCareCenters
                .Include(c => c.Staff)
                .FirstOrDefaultAsync(c => c.Id == centerId);
            return center?.Staff ?? new List<Staff>();
        }

        public async Task<Staff> CreateAsync(StaffRequestDTO dto)
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

            if (dto.HealthCareCenterIds?.Any() == true)
            {
                var centers = await _context.HealthCareCenters
                    .Where(c => dto.HealthCareCenterIds.Contains(c.Id))
                    .ToListAsync();
                staff.HealthCareCenters = centers;
            }

            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null) return false;
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Staff?> GetByEmailAsync(string email)
        {
            return await _context.Staff.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Staff> UpdateAsync(int id, StaffRequestDTO dto)
        {
            var staff = await _context.Staff
                .Include(s => s.ContactDetails)
                .Include(s => s.AddressDetails)
                .Include(s => s.HealthCareCenters)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (staff == null) throw new Exception("Staff member not found.");
            staff.FirstName = dto.FirstName;
            staff.LastName = dto.LastName;
            staff.Email = dto.Email;
            staff.PasswordHash = _passwordService.HashPassword(dto.Password);
            staff.DateOfBirth = dto.DateOfBirth;
            staff.ContactDetails = dto.ContactDetails;
            staff.AddressDetails = dto.AddressDetails;
            staff.Salary = dto.Salary;
            staff.JobTitle = dto.JobTitle;
            if (dto.HealthCareCenterIds?.Any() == true)
            {
                var centers = await _context.HealthCareCenters
                    .Where(c => dto.HealthCareCenterIds.Contains(c.Id))
                    .ToListAsync();
                staff.HealthCareCenters = centers;
            }
            await _context.SaveChangesAsync();
            return staff;
        }
    }
}
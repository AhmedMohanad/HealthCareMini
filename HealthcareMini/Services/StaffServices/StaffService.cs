// Services/StaffServices/StaffService.cs
using HealthcareMini.Data;
using HealthcareMini.DTOs.Staff;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.StaffServices
{
    public class StaffService
    {
        private readonly HealthcareDbContext _context;

        public StaffService(HealthcareDbContext context)
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
                PasswordHash = dto.PasswordHash,
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
    }
}
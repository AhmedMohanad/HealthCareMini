// Services/AdminServices/AdminService.cs
using HealthcareMini.Data;
using HealthcareMini.DTOs.Admin;
using HealthcareMini.DTOs.AdminDTO;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.AdminServices
{
    public class AdminService
    {
        private readonly HealthcareDbContext _context;

        public AdminService(HealthcareDbContext context)
        {
            _context = context;
        }

        public async Task<Admin?> GetByIdAsync(int id)
        {
            return await _context.Admins
                .Include(a => a.ContactDetails)
                .Include(a => a.AddressDetails)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _context.Admins
                .Include(a => a.ContactDetails)
                .Include(a => a.AddressDetails)
                .ToListAsync();
        }

        public async Task<Admin> CreateAsync(AdminRequestDTO dto)
        {
            var admin = new Admin
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                DateOfBirth = dto.DateOfBirth,
                ContactDetails = dto.ContactDetails,
                AddressDetails = dto.AddressDetails,
                Role = UserRole.Admin
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return false;
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
// Services/ReceptionistServices/ReceptionistService.cs
using HealthcareMini.Data;
using HealthcareMini.DTOs.Receptionist;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Services.PasswordServices;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.ReceptionistServices
{
    public class ReceptionistService : IReceptionistService 
    { 
        private readonly HealthcareDbContext _context;
        private readonly IPasswordService _passwordService;

        public ReceptionistService(HealthcareDbContext context, IPasswordService passwordService    )
        {
            _context = context;
        }

        public async Task<Receptionist?> GetByIdAsync(int id)
        {
            return await _context.Receptionists
                .Include(r => r.ContactDetails)
                .Include(r => r.AddressDetails)
                .Include(r => r.HealthCareCenters)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Receptionist>> GetAllAsync()
        {
            return await _context.Receptionists
                .Include(r => r.ContactDetails)
                .Include(r => r.HealthCareCenters)
                .ToListAsync();
        }

        public async Task<IEnumerable<Receptionist>> GetReceptionistsByCenterAsync(int centerId)
        {
            var center = await _context.HealthCareCenters
                .Include(c => c.Receptionists)
                .FirstOrDefaultAsync(c => c.Id == centerId);
            return center?.Receptionists ?? new List<Receptionist>();
        }

        public async Task<Receptionist> CreateAsync(ReceptionistRequestDTO dto)
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

            if (dto.HealthCareCenterIds?.Any() == true)
            {
                var centers = await _context.HealthCareCenters
                    .Where(c => dto.HealthCareCenterIds.Contains(c.Id))
                    .ToListAsync();
                receptionist.HealthCareCenters = centers;
            }

            _context.Receptionists.Add(receptionist);
            await _context.SaveChangesAsync();
            return receptionist;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var receptionist = await _context.Receptionists.FindAsync(id);
            if (receptionist == null) return false;
            _context.Receptionists.Remove(receptionist);
            await _context.SaveChangesAsync();
            return true;
        }


        public Task<Receptionist> UpdateAsync(int id, ReceptionistRequestDTO dto)
        {
           var receptionist = _context.Receptionists
                .Include(r => r.ContactDetails)
                .Include(r => r.AddressDetails)
                .Include(r => r.HealthCareCenters)
                .FirstOrDefault(r => r.Id == id);
            if (receptionist == null) return Task.FromResult<Receptionist>(null);
            receptionist.FirstName = dto.FirstName;
            receptionist.LastName = dto.LastName;
            receptionist.Email = dto.Email;
            receptionist.PasswordHash = _passwordService.HashPassword(dto.Password);
            receptionist.DateOfBirth = dto.DateOfBirth;
            receptionist.ContactDetails = dto.ContactDetails;
            receptionist.AddressDetails = dto.AddressDetails;
            receptionist.Salary = dto.Salary;
            if (dto.HealthCareCenterIds?.Any() == true)
            {
                var centers = _context.HealthCareCenters
                    .Where(c => dto.HealthCareCenterIds.Contains(c.Id))
                    .ToList();
                receptionist.HealthCareCenters = centers;
            }
            _context.Receptionists.Update(receptionist);
            _context.SaveChanges();
            return Task.FromResult(receptionist);
        }

    }
}
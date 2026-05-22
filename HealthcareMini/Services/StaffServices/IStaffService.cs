using HealthcareMini.DTOs.Staff;
using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.StaffServices
{
    public interface IStaffService
    {
        Task<Staff?> GetByIdAsync(int id);
        Task<IEnumerable<Staff>> GetAllAsync();
        Task<IEnumerable<Staff>> GetStaffByCenterAsync(int centerId);
        Task<Staff> CreateAsync(StaffRequestDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<Staff?> GetByEmailAsync(string email);
        Task<Staff> UpdateAsync(int id, StaffRequestDTO dto);
    }
}
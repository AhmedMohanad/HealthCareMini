using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Interfaces;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    public interface IHealthCareCenterServices
    {
        Task<ResponsCenter> CreateAsync(CreateCenterDTO healthcareCenter);
        Task<ResponsCenter?> EditAsync(int id, EditCenterDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<ResponsCenter?> GetByIdAsync(int id);
        Task<ResponsCenter?> GetByEmailAsync(string email);
        Task<IEnumerable<LimitedResponsCenter>> GetAllAsync();
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<LimitedResponsCenter?> GetByNameAsync(string name);
        Task<IEnumerable<IEmployee>> GetEmployeesAsync(int centerId);
        Task<IEnumerable<LimitedResponsCenter>> GetLimitedAsync();
        Task<bool> DeleteByEmailAsync(string email);
    }
}
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Interfaces;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    public interface IHealthCareCenterQueryService
    {
        Task<ResponsCenter?> GetByIdAsync(int id);
        Task<ResponsCenter?> GetByEmailAsync(string email);
        Task<IEnumerable<LimitedResponsCenter>> GetAllAsync();
        Task<IEnumerable<LimitedResponsCenter>> GetLimitedAsync();
        Task<LimitedResponsCenter?> GetByNameAsync(string name);
        Task<IEnumerable<IEmployee>> GetEmployeesAsync(int centerId);
    }
}
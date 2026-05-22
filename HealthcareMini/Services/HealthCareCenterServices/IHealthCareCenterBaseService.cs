using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    public interface IHealthCareCenterBaseService
    {
        Task<ResponsCenter> CreateAsync(CreateCenterDTO healthcareCenter);
        Task<ResponsCenter?> EditAsync(int id, EditCenterDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByEmailAsync(string email);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
    }
}
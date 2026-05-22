using HealthcareMini.DTOs.Receptionist;
using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.ReceptionistServices
{
    public interface IReceptionistService
    {
        Task<Receptionist?> GetByIdAsync(int id);
        Task<IEnumerable<Receptionist>> GetAllAsync();
        Task<IEnumerable<Receptionist>> GetReceptionistsByCenterAsync(int centerId);
        Task<Receptionist> CreateAsync(ReceptionistRequestDTO dto);
        Task<bool> DeleteAsync(int id);

        Task<Receptionist> UpdateAsync(int id, ReceptionistRequestDTO dto);
    }
}
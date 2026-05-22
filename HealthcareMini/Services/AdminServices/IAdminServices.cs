using HealthcareMini.DTOs.AdminDTO;
using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.AdminServices
{
    /// <summary>
    /// What an admin service should be able to do
    /// Nothing fancy - just the basics: fetch, create, delete
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Grab an admin by their ID
        /// Returns null if nobody exists with that ID
        /// </summary>
        Task<Admin?> GetByIdAsync(int id);

        /// <summary>
        /// Find an admin using their email address
        /// Useful for login checks and duplicate detection
        /// </summary>
        Task<Admin?> GetByEmailAsync(string email);

        /// <summary>
        /// Get every admin in the system
        /// Use sparingly - could be a lot of data
        /// </summary>
        Task<IEnumerable<Admin>> GetAllAsync();

        /// <summary>
        /// Add a new admin to the database
        /// Takes the DTO, creates the entity, saves it
        /// </summary>
        Task<Admin> CreateAsync(AdminRequestDTO dto);

        /// <summary>
        /// Remove an admin from the system
        /// Returns true if we deleted someone, false if ID wasn't found
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
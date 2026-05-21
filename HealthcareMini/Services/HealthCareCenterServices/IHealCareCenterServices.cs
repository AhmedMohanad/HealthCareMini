using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Interfaces;


   

namespace HealthcareMini.Services.HealthCareCenterServices
    {
        public interface IHealthCareCenterServices
        {
            // Creates a new health care center
            Task<HealthCareCenter> CreateAsync(CreateCenterDTO healthcareCenter);

            // Edits an existing health care center by ID
            Task<HealthCareCenter?> EditAsync(int id, EditCenterDTO dto);

            // Deletes a health care center by ID
            Task<bool> DeleteAsync(int id);

            //get a health care center by id
            Task<HealthCareCenter?> GetByIdAsync(int id);

            // get all health care centers
            Task<IEnumerable<HealthCareCenter>> GetAllAsync();

            //get a health care center by email
            Task<HealthCareCenter?> GetByEmailAsync(string email);

            // get a health care center by name
            Task<HealthCareCenter?> GetByNameAsync(string name);

            // change the status of the center to active (IsActive = true)
            Task<bool> ActivateAsync(int id);

            // change the status of the center to inactive (IsActive = false)
            Task<bool> DeactivateAsync(int id);

            // get all employees of a health care center
            Task<IEnumerable<IEmployee>> GetEmployeesAsync(int centerId);
        }
    }


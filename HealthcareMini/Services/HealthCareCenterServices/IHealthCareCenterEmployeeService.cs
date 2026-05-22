using HealthcareMini.DTOs.Doctor;
using HealthcareMini.DTOs.Receptionist;
using HealthcareMini.DTOs.Staff;
using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    public interface IHealthCareCenterEmployeeService
    {
        // Doctor Management
        Task<Doctor?> AddDoctorAsync(int centerId, DoctorRequestDTO dto);
        Task<Doctor?> AddDoctorAsync(int centerId, Doctor doctor);
        Task<IEnumerable<Doctor>> AddDoctorsAsync(int centerId, IEnumerable<Doctor> doctors);
        Task<bool> RemoveDoctorFromCenterAsync(int centerId, int doctorId);

        // Receptionist Management
        Task<Receptionist?> AddReceptionistAsync(int centerId, ReceptionistRequestDTO dto);
        Task<Receptionist?> AddReceptionistAsync(int centerId, Receptionist receptionist);
        Task<IEnumerable<Receptionist>> AddReceptionistsAsync(int centerId, IEnumerable<Receptionist> receptionists);
        Task<bool> RemoveReceptionistFromCenterAsync(int centerId, int receptionistId);

        // Staff Management
        Task<Staff?> AddStaffAsync(int centerId, StaffRequestDTO dto);
        Task<Staff?> AddStaffAsync(int centerId, Staff staff);
        Task<IEnumerable<Staff>> AddStaffAsync(int centerId, IEnumerable<Staff> staffList);
        Task<bool> RemoveStaffFromCenterAsync(int centerId, int staffId);
    }
}
using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Interfaces;

namespace HealthcareMini.Services.HealthCareCenterServices
{

    //Facade service that combines core operations
    public class HealthCareCenterServices : IHealthCareCenterServices
    {
        private readonly HealthCareCenterBaseService _coreService;
        private readonly HealthCareCenterQueryService _queryService;
        private readonly HealthCareCenterEmployeeService _employeeService;
        private readonly HealthCareCenterAppointmentService _appointmentService;

        public HealthCareCenterServices(HealthcareDbContext context)
        {
            _coreService = new HealthCareCenterBaseService(context);
            _queryService = new HealthCareCenterQueryService(context);
            _employeeService = new HealthCareCenterEmployeeService(context);
            _appointmentService = new HealthCareCenterAppointmentService(context);
        }

        // Core Operations
        public async Task<ResponsCenter> CreateAsync(CreateCenterDTO dto)
            => await _coreService.CreateAsync(dto);

        public async Task<ResponsCenter?> EditAsync(int id, EditCenterDTO dto)
            => await _coreService.EditAsync(id, dto);

        public async Task<bool> DeleteAsync(int id)
            => await _coreService.DeleteAsync(id);

        public async Task<bool> DeleteByEmailAsync(string email)
            => await _coreService.DeleteByEmailAsync(email);

        public async Task<bool> ActivateAsync(int id)
            => await _coreService.ActivateAsync(id);

        public async Task<bool> DeactivateAsync(int id)
            => await _coreService.DeactivateAsync(id);

        // Query Operations
        public async Task<ResponsCenter?> GetByIdAsync(int id)
            => await _queryService.GetByIdAsync(id);

        public async Task<ResponsCenter?> GetByEmailAsync(string email)
            => await _queryService.GetByEmailAsync(email);

        public async Task<IEnumerable<LimitedResponsCenter>> GetAllAsync()
            => await _queryService.GetAllAsync();

        public async Task<LimitedResponsCenter?> GetByNameAsync(string name)
            => await _queryService.GetByNameAsync(name);

        public async Task<IEnumerable<LimitedResponsCenter>> GetLimitedAsync()
            => await _queryService.GetLimitedAsync();

        public async Task<IEnumerable<IEmployee>> GetEmployeesAsync(int centerId)
            => await _queryService.GetEmployeesAsync(centerId);

        // Employee Management
        public async Task<Doctor?> AddDoctorAsync(int centerId, Doctor doctor)
            => await _employeeService.AddDoctorAsync(centerId, doctor);

        public async Task<IEnumerable<Doctor>> AddDoctorsAsync(int centerId, IEnumerable<Doctor> doctors)
            => await _employeeService.AddDoctorsAsync(centerId, doctors);

        public async Task<bool> RemoveDoctorAsync(int centerId, int doctorId)
            => await _employeeService.RemoveDoctorFromCenterAsync(centerId, doctorId);

        public async Task<Receptionist?> AddReceptionistAsync(int centerId, Receptionist receptionist)
            => await _employeeService.AddReceptionistAsync(centerId, receptionist);

        public async Task<IEnumerable<Receptionist>> AddReceptionistsAsync(int centerId, IEnumerable<Receptionist> receptionists)
            => await _employeeService.AddReceptionistsAsync(centerId, receptionists);

        public async Task<bool> RemoveReceptionistAsync(int centerId, int receptionistId)
            => await _employeeService.RemoveReceptionistFromCenterAsync(centerId, receptionistId);

        public async Task<Staff?> AddStaffAsync(int centerId, Staff staff)
            => await _employeeService.AddStaffAsync(centerId, staff);

        public async Task<IEnumerable<Staff>> AddStaffAsync(int centerId, IEnumerable<Staff> staffList)
            => await _employeeService.AddStaffAsync(centerId, staffList);

        public async Task<bool> RemoveStaffAsync(int centerId, int staffId)
            => await _employeeService.RemoveStaffFromCenterAsync(centerId, staffId);

        // Appointment Management
        public async Task<Appointment?> AddAppointmentAsync(int centerId, Appointment appointment)
            => await _appointmentService.AddAppointmentAsync(centerId, appointment);

        public async Task<IEnumerable<Appointment>> AddAppointmentsAsync(int centerId, IEnumerable<Appointment> appointments)
            => await _appointmentService.AddAppointmentsAsync(centerId, appointments);

        public async Task<bool> RemoveAppointmentAsync(int centerId, int appointmentId)
            => await _appointmentService.RemoveAppointmentFromCenterAsync(centerId, appointmentId);
    }
}
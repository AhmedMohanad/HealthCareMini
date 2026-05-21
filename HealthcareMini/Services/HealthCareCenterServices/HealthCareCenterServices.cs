using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Interfaces;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    // Facade that combines core operations.
    // Sub-services are injected by DI — never instantiated with "new" here,
    // which would bypass the DI container and break scoped lifetime guarantees.
    public class HealthCareCenterServices : IHealthCareCenterServices
    {
        private readonly HealthCareCenterBaseService _coreService;
        private readonly HealthCareCenterQueryService _queryService;
        private readonly HealthCareCenterEmployeeService _employeeService;
        private readonly HealthCareCenterAppointmentService _appointmentService;

        public HealthCareCenterServices(
            HealthCareCenterBaseService coreService,
            HealthCareCenterQueryService queryService,
            HealthCareCenterEmployeeService employeeService,
            HealthCareCenterAppointmentService appointmentService)
        {
            _coreService = coreService;
            _queryService = queryService;
            _employeeService = employeeService;
            _appointmentService = appointmentService;
        }

        // ── Core Operations ───────────────────────────────────────────────────
        public Task<ResponsCenter> CreateAsync(CreateCenterDTO dto)
            => _coreService.CreateAsync(dto);

        public Task<ResponsCenter?> EditAsync(int id, EditCenterDTO dto)
            => _coreService.EditAsync(id, dto);

        public Task<bool> DeleteAsync(int id)
            => _coreService.DeleteAsync(id);

        public Task<bool> DeleteByEmailAsync(string email)
            => _coreService.DeleteByEmailAsync(email);

        public Task<bool> ActivateAsync(int id)
            => _coreService.ActivateAsync(id);

        public Task<bool> DeactivateAsync(int id)
            => _coreService.DeactivateAsync(id);

        // ── Query Operations ──────────────────────────────────────────────────
        public Task<ResponsCenter?> GetByIdAsync(int id)
            => _queryService.GetByIdAsync(id);

        public Task<ResponsCenter?> GetByEmailAsync(string email)
            => _queryService.GetByEmailAsync(email);

        public Task<IEnumerable<LimitedResponsCenter>> GetAllAsync()
            => _queryService.GetAllAsync();

        public Task<LimitedResponsCenter?> GetByNameAsync(string name)
            => _queryService.GetByNameAsync(name);

        public Task<IEnumerable<LimitedResponsCenter>> GetLimitedAsync()
            => _queryService.GetLimitedAsync();

        public Task<IEnumerable<IEmployee>> GetEmployeesAsync(int centerId)
            => _queryService.GetEmployeesAsync(centerId);

        // ── Employee Management ───────────────────────────────────────────────
        public Task<Doctor?> AddDoctorAsync(int centerId, Doctor doctor)
            => _employeeService.AddDoctorAsync(centerId, doctor);

        public Task<IEnumerable<Doctor>> AddDoctorsAsync(int centerId, IEnumerable<Doctor> doctors)
            => _employeeService.AddDoctorsAsync(centerId, doctors);

        public Task<bool> RemoveDoctorAsync(int centerId, int doctorId)
            => _employeeService.RemoveDoctorFromCenterAsync(centerId, doctorId);

        public Task<Receptionist?> AddReceptionistAsync(int centerId, Receptionist receptionist)
            => _employeeService.AddReceptionistAsync(centerId, receptionist);

        public Task<IEnumerable<Receptionist>> AddReceptionistsAsync(int centerId, IEnumerable<Receptionist> receptionists)
            => _employeeService.AddReceptionistsAsync(centerId, receptionists);

        public Task<bool> RemoveReceptionistAsync(int centerId, int receptionistId)
            => _employeeService.RemoveReceptionistFromCenterAsync(centerId, receptionistId);

        public Task<Staff?> AddStaffAsync(int centerId, Staff staff)
            => _employeeService.AddStaffAsync(centerId, staff);

        public Task<IEnumerable<Staff>> AddStaffAsync(int centerId, IEnumerable<Staff> staffList)
            => _employeeService.AddStaffAsync(centerId, staffList);

        public Task<bool> RemoveStaffAsync(int centerId, int staffId)
            => _employeeService.RemoveStaffFromCenterAsync(centerId, staffId);

        // ── Appointment Management ────────────────────────────────────────────
        public Task<Appointment?> AddAppointmentAsync(int centerId, Appointment appointment)
            => _appointmentService.AddAppointmentAsync(centerId, appointment);

        public Task<IEnumerable<Appointment>> AddAppointmentsAsync(int centerId, IEnumerable<Appointment> appointments)
            => _appointmentService.AddAppointmentsAsync(centerId, appointments);

        public Task<bool> RemoveAppointmentAsync(int centerId, int appointmentId)
            => _appointmentService.RemoveAppointmentFromCenterAsync(centerId, appointmentId);
    }
}
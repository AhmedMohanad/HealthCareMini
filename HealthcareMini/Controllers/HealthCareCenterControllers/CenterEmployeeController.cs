using FluentValidation;
using HealthcareMini.DTOs.Doctor;
using HealthcareMini.DTOs.Receptionist;
using HealthcareMini.DTOs.Staff;
using HealthcareMini.Services.DoctorServices;
using HealthcareMini.Services.HealthCareCenterServices;
using HealthcareMini.Services.ReceptionistServices;
using HealthcareMini.Services.StaffServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareMini.Controllers
{
    /// <summary>
    /// Center Employee Controller - Manages employees within a healthcare center
    /// Rule B: HealthCareCenter can do anything in HIS center (create/update employees, 
    /// but cannot delete the center itself or modify other centers)
    /// Rule C: Receptionist can manage records, staff, patients (no delete)
    /// Rule D: Staff can only view medical records and centers/doctors
    /// Rule E: Patient data is protected - only doctors, receptionists, and center admins can see
    /// </summary>
    [Route("api/center/employees")]
    [ApiController]
    [Authorize(Roles = "HealthCareCenter,Receptionist,Staff")]
    public class CenterEmployeeController : ControllerBase
    {
        private readonly HealthCareCenterEmployeeService _employeeService;
        private readonly DoctorService _doctorService;
        private readonly ReceptionistService _receptionistService;
        private readonly StaffService _staffService;
        private readonly HealthCareCenterQueryService _centerQueryService;

        public CenterEmployeeController(
            HealthCareCenterEmployeeService employeeService,
            DoctorService doctorService,
            ReceptionistService receptionistService,
            StaffService staffService,
            HealthCareCenterQueryService centerQueryService)
        {
            _employeeService = employeeService;
            _doctorService = doctorService;
            _receptionistService = receptionistService;
            _staffService = staffService;
            _centerQueryService = centerQueryService;
        }

        /// <summary>
        /// Gets the current authenticated center's ID
        /// </summary>
        private async Task<int> GetCurrentCenterIdAsync()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(email))
                return 0;

            var center = await _centerQueryService.GetByEmailAsync(email!);
            return center?.Id ?? 0;
        }

        /// <summary>
        /// Checks if current user has write permissions (not Staff)
        /// Staff can only READ, not WRITE
        /// </summary>
        private bool HasWritePermission()
        {
            return User.IsInRole("HealthCareCenter") || User.IsInRole("Receptionist");
        }

        #region Doctors

        /// <summary>
        /// GET: api/center/employees/doctors
        /// Access: HealthCareCenter, Receptionist, Staff (all can view)
        /// </summary>
        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized(new { message = "Center not found for authenticated user." });

            var doctors = await _doctorService.GetDoctorsByCenterAsync(centerId);
            return Ok(doctors);
        }

        /// <summary>
        /// POST: api/center/employees/doctors
        /// Access: HealthCareCenter, Receptionist (Staff cannot create)
        /// </summary>
        [HttpPost("doctors")]
        [Authorize(Roles = "HealthCareCenter,Receptionist")]
        public async Task<IActionResult> AddDoctor(
            [FromBody] DoctorRequestDTO dto,
            [FromServices] IValidator<DoctorRequestDTO> validator)
        {
            if (!HasWritePermission())
                return Forbid("You don't have permission to add doctors.");

            if (dto is null)
                return BadRequest(new { message = "Invalid doctor data." });

            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized(new { message = "Center not found for authenticated user." });

            var result = await _employeeService.AddDoctorAsync(centerId, dto);
            return result is null
                ? NotFound(new { message = "Center not found." })
                : Ok(new { message = "Doctor added successfully.", doctor = result });
        }

        /// <summary>
        /// DELETE: api/center/employees/doctors/{doctorId}
        /// Access: HealthCareCenter ONLY (Receptionist cannot delete)
        /// </summary>
        [HttpDelete("doctors/{doctorId:int}")]
        [Authorize(Roles = "HealthCareCenter")]
        public async Task<IActionResult> RemoveDoctor(int doctorId)
        {
            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized(new { message = "Center not found for authenticated user." });

            var result = await _employeeService.RemoveDoctorFromCenterAsync(centerId, doctorId);
            return result
                ? Ok(new { message = "Doctor removed successfully." })
                : NotFound(new { message = "Doctor or center not found." });
        }

        #endregion

        #region Receptionists

        [HttpGet("receptionists")]
        public async Task<IActionResult> GetReceptionists()
        {
            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized(new { message = "Center not found for authenticated user." });

            var receptionists = await _receptionistService.GetReceptionistsByCenterAsync(centerId);
            return Ok(receptionists);
        }

        [HttpPost("receptionists")]
        [Authorize(Roles = "HealthCareCenter")]
        public async Task<IActionResult> AddReceptionist(
            [FromBody] ReceptionistRequestDTO dto,
            [FromServices] IValidator<ReceptionistRequestDTO> validator)
        {
            if (dto is null)
                return BadRequest(new { message = "Invalid receptionist data." });

            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized(new { message = "Center not found for authenticated user." });

            var result = await _employeeService.AddReceptionistAsync(centerId, dto);
            return result is null
                ? NotFound(new { message = "Center not found." })
                : Ok(new { message = "Receptionist added successfully.", receptionist = result });
        }

        [HttpDelete("receptionists/{receptionistId:int}")]
        [Authorize(Roles = "HealthCareCenter")]
        public async Task<IActionResult> RemoveReceptionist(int receptionistId)
        {
            var centerId = await GetCurrentCenterIdAsync();
            var result = await _employeeService.RemoveReceptionistFromCenterAsync(centerId, receptionistId);
            return result
                ? Ok(new { message = "Receptionist removed successfully." })
                : NotFound(new { message = "Receptionist or center not found." });
        }

        #endregion

        #region Staff

        [HttpGet("staff")]
        public async Task<IActionResult> GetStaff()
        {
            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized(new { message = "Center not found for authenticated user." });

            var staff = await _staffService.GetStaffByCenterAsync(centerId);
            return Ok(staff);
        }

        [HttpPost("staff")]
        [Authorize(Roles = "HealthCareCenter")]
        public async Task<IActionResult> AddStaff(
            [FromBody] StaffRequestDTO dto,
            [FromServices] IValidator<StaffRequestDTO> validator)
        {
            if (dto is null)
                return BadRequest(new { message = "Invalid staff data." });

            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized(new { message = "Center not found for authenticated user." });

            var result = await _employeeService.AddStaffAsync(centerId, dto);
            return result is null
                ? NotFound(new { message = "Center not found." })
                : Ok(new { message = "Staff member added successfully.", staff = result });
        }

        [HttpDelete("staff/{staffId:int}")]
        [Authorize(Roles = "HealthCareCenter")]
        public async Task<IActionResult> RemoveStaff(int staffId)
        {
            var centerId = await GetCurrentCenterIdAsync();
            var result = await _employeeService.RemoveStaffFromCenterAsync(centerId, staffId);
            return result
                ? Ok(new { message = "Staff member removed successfully." })
                : NotFound(new { message = "Staff member or center not found." });
        }

        #endregion
    }
}
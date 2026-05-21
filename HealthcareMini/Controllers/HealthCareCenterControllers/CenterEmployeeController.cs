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
    [Route("api/center/employees")]
    [ApiController]
    [Authorize(Roles = "HealthCareCenter")]
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

        // Returns the ID of the currently authenticated health care center.
        private async Task<int> GetCurrentCenterIdAsync()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var center = await _centerQueryService.GetByEmailAsync(email!);
            return center?.Id ?? 0;
        }

        // ─────────────────────────────────────────────────────────────────────
        // DOCTORS
        // ─────────────────────────────────────────────────────────────────────

        // GET api/center/employees/doctors
        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var centerId = await GetCurrentCenterIdAsync();
            var doctors = await _doctorService.GetDoctorsByCenterAsync(centerId);
            return Ok(doctors);
        }

        // POST api/center/employees/doctors
        // Previously passed DoctorRequestDTO directly to a service method that
        // expected a Doctor entity — compile error / runtime crash.
        // Now calls the DTO-accepting overload added to HealthCareCenterEmployeeService.
        [HttpPost("doctors")]
        public async Task<IActionResult> AddDoctor(
            [FromBody] DoctorRequestDTO dto,
            [FromServices] IValidator<DoctorRequestDTO> validator)
        {
            if (dto is null)
                return BadRequest("Invalid doctor data.");

            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized("Center not found for the authenticated user.");

            var result = await _employeeService.AddDoctorAsync(centerId, dto);
            return result is null
                ? NotFound(new { message = "Center not found." })
                : Ok(new { message = "Doctor added successfully.", doctor = result });
        }

        // DELETE api/center/employees/doctors/{doctorId}
        [HttpDelete("doctors/{doctorId:int}")]
        public async Task<IActionResult> RemoveDoctor(int doctorId)
        {
            var centerId = await GetCurrentCenterIdAsync();
            var result = await _employeeService.RemoveDoctorFromCenterAsync(centerId, doctorId);
            return result
                ? Ok(new { message = "Doctor removed successfully." })
                : NotFound(new { message = "Doctor or center not found." });
        }

        // ─────────────────────────────────────────────────────────────────────
        // RECEPTIONISTS
        // ─────────────────────────────────────────────────────────────────────

        // GET api/center/employees/receptionists
        [HttpGet("receptionists")]
        public async Task<IActionResult> GetReceptionists()
        {
            var centerId = await GetCurrentCenterIdAsync();
            var receptionists = await _receptionistService.GetReceptionistsByCenterAsync(centerId);
            return Ok(receptionists);
        }

        // POST api/center/employees/receptionists
        // Same fix as AddDoctor — DTO-accepting overload used instead of entity method.
        [HttpPost("receptionists")]
        public async Task<IActionResult> AddReceptionist(
            [FromBody] ReceptionistRequestDTO dto,
            [FromServices] IValidator<ReceptionistRequestDTO> validator)
        {
            if (dto is null)
                return BadRequest("Invalid receptionist data.");

            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized("Center not found for the authenticated user.");

            var result = await _employeeService.AddReceptionistAsync(centerId, dto);
            return result is null
                ? NotFound(new { message = "Center not found." })
                : Ok(new { message = "Receptionist added successfully.", receptionist = result });
        }

        // DELETE api/center/employees/receptionists/{receptionistId}
        [HttpDelete("receptionists/{receptionistId:int}")]
        public async Task<IActionResult> RemoveReceptionist(int receptionistId)
        {
            var centerId = await GetCurrentCenterIdAsync();
            var result = await _employeeService.RemoveReceptionistFromCenterAsync(centerId, receptionistId);
            return result
                ? Ok(new { message = "Receptionist removed successfully." })
                : NotFound(new { message = "Receptionist or center not found." });
        }

        // ─────────────────────────────────────────────────────────────────────
        // STAFF
        // ─────────────────────────────────────────────────────────────────────

        // GET api/center/employees/staff
        [HttpGet("staff")]
        public async Task<IActionResult> GetStaff()
        {
            var centerId = await GetCurrentCenterIdAsync();
            var staff = await _staffService.GetStaffByCenterAsync(centerId);
            return Ok(staff);
        }

        // POST api/center/employees/staff
        // BUG 1: [HttpPost("staff")] attribute was MISSING — the method was
        //         unreachable by any HTTP request.
        // BUG 2: Same DTO/entity type mismatch as AddDoctor; fixed with DTO overload.
        [HttpPost("staff")]
        public async Task<IActionResult> AddStaff(
            [FromBody] StaffRequestDTO dto,
            [FromServices] IValidator<StaffRequestDTO> validator)
        {
            if (dto is null)
                return BadRequest("Invalid staff data.");

            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var centerId = await GetCurrentCenterIdAsync();
            if (centerId == 0)
                return Unauthorized("Center not found for the authenticated user.");

            var result = await _employeeService.AddStaffAsync(centerId, dto);
            return result is null
                ? NotFound(new { message = "Center not found." })
                : Ok(new { message = "Staff member added successfully.", staff = result });
        }

        // DELETE api/center/employees/staff/{staffId}
        [HttpDelete("staff/{staffId:int}")]
        public async Task<IActionResult> RemoveStaff(int staffId)
        {
            var centerId = await GetCurrentCenterIdAsync();
            var result = await _employeeService.RemoveStaffFromCenterAsync(centerId, staffId);
            return result
                ? Ok(new { message = "Staff member removed successfully." })
                : NotFound(new { message = "Staff member or center not found." });
        }
    }
}
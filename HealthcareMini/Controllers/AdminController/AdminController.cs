using FluentValidation;
using HealthcareMini.DTOs.AdminDTO;
using HealthcareMini.DTOs.Doctor;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.DTOs.Patient;
using HealthcareMini.DTOs.Receptionist;
using HealthcareMini.DTOs.Staff;
using HealthcareMini.Services.AdminServices;
using HealthcareMini.Services.DoctorServices;
using HealthcareMini.Services.HealthCareCenterServices;
using HealthcareMini.Services.PatientServices;
using HealthcareMini.Services.ReceptionistServices;
using HealthcareMini.Services.StaffServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareMini.Controllers.Admin
{
    
    [Route("api/admin")]
    [ApiController]
    // [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly DoctorService _doctorService;
        private readonly ReceptionistService _receptionistService;
        private readonly StaffService _staffService;
        private readonly PatientService _patientService;
        private readonly HealthCareCenterBaseService _centerCoreService;
        private readonly HealthCareCenterQueryService _centerQueryService;

        public AdminController(
            IAdminService adminService,
            DoctorService doctorService,
            ReceptionistService receptionistService,
            StaffService staffService,
            PatientService patientService,
            HealthCareCenterBaseService centerCoreService,
            HealthCareCenterQueryService centerQueryService)
        {
            _adminService = adminService;
            _doctorService = doctorService;
            _receptionistService = receptionistService;
            _staffService = staffService;
            _patientService = patientService;
            _centerCoreService = centerCoreService;
            _centerQueryService = centerQueryService;
        }

        #region Admin Management

        /// <summary>
        /// Creates a new admin user
        /// POST: api/admin
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(
            [FromBody] AdminRequestDTO dto,
            [FromServices] IValidator<AdminRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var existingAdmin = await _adminService.GetByEmailAsync(dto.Email);
            if (existingAdmin != null)
                return Conflict(new { message = "Admin with this email already exists." });

            var result = await _adminService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAdminById), new { id = result.Id },
                new { message = "Admin created successfully.", admin = result });
        }

        /// <summary>
        /// Gets all admins
        /// GET: api/admin/admins
        /// </summary>
        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAsync();
            return Ok(admins);
        }

        /// <summary>
        /// Gets admin by ID
        /// GET: api/admin/admins/{id}
        /// </summary>
        [HttpGet("admins/{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _adminService.GetByIdAsync(id);
            if (admin == null)
                return NotFound(new { message = "Admin not found." });
            return Ok(admin);
        }

        /// <summary>
        /// Deletes an admin
        /// DELETE: api/admin/admins/{id}
        /// </summary>
        [HttpDelete("admins/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var result = await _adminService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Admin not found." });
            return Ok(new { message = "Admin deleted successfully." });
        }

        #endregion

        #region Health Care Center Management

        [HttpGet("centers")]
        public async Task<IActionResult> GetAllCenters()
        {
            var centers = await _centerQueryService.GetAllAsync();
            return Ok(centers);
        }

        [HttpGet("centers/{id}")]
        public async Task<IActionResult> GetCenterById(int id)
        {
            var center = await _centerQueryService.GetByIdAsync(id);
            if (center == null)
                return NotFound(new { message = "Health care center not found." });
            return Ok(center);
        }

        [HttpPost("centers")]
        public async Task<IActionResult> CreateCenter(
            [FromBody] CreateCenterDTO dto,
            [FromServices] IValidator<CreateCenterDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _centerCoreService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetCenterById), new { id = result.Id },
                new { message = "Health care center created successfully.", center = result });
        }

        [HttpPut("centers/{id}")]
        public async Task<IActionResult> UpdateCenter(
            int id,
            [FromBody] EditCenterDTO dto,
            [FromServices] IValidator<EditCenterDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _centerCoreService.EditAsync(id, dto);
            if (result == null)
                return NotFound(new { message = "Health care center not found." });
            return Ok(new { message = "Health care center updated successfully.", center = result });
        }

        [HttpDelete("centers/{id}")]
        public async Task<IActionResult> DeleteCenter(int id)
        {
            var result = await _centerCoreService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Health care center not found." });
            return Ok(new { message = "Health care center deleted successfully." });
        }

        [HttpPost("centers/{id}/activate")]
        public async Task<IActionResult> ActivateCenter(int id)
        {
            var result = await _centerCoreService.ActivateAsync(id);
            if (!result)
                return NotFound(new { message = "Health care center not found." });
            return Ok(new { message = "Health care center activated successfully." });
        }

        [HttpPost("centers/{id}/deactivate")]
        public async Task<IActionResult> DeactivateCenter(int id)
        {
            var result = await _centerCoreService.DeactivateAsync(id);
            if (!result)
                return NotFound(new { message = "Health care center not found." });
            return Ok(new { message = "Health care center deactivated successfully." });
        }

        #endregion

        #region Doctor Management

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);
        }

        [HttpGet("doctors/{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found." });
            return Ok(doctor);
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor(
            [FromBody] DoctorRequestDTO dto,
            [FromServices] IValidator<DoctorRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _doctorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetDoctorById), new { id = result.Id },
                new { message = "Doctor created successfully.", doctor = result });
        }

        [HttpPut("doctors/{id}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            [FromBody] DoctorRequestDTO dto,
            [FromServices] IValidator<DoctorRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _doctorService.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(new { message = "Doctor not found." });
            return Ok(new { message = "Doctor updated successfully.", doctor = result });
        }

        [HttpDelete("doctors/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var result = await _doctorService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Doctor not found." });
            return Ok(new { message = "Doctor deleted successfully." });
        }

        #endregion

        #region Receptionist Management

        [HttpGet("receptionists")]
        public async Task<IActionResult> GetAllReceptionists()
        {
            var receptionists = await _receptionistService.GetAllAsync();
            return Ok(receptionists);
        }

        [HttpPost("receptionists")]
        public async Task<IActionResult> CreateReceptionist(
            [FromBody] ReceptionistRequestDTO dto,
            [FromServices] IValidator<ReceptionistRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _receptionistService.CreateAsync(dto);
            return Ok(new { message = "Receptionist created successfully.", receptionist = result });
        }

        [HttpDelete("receptionists/{id}")]
        public async Task<IActionResult> DeleteReceptionist(int id)
        {
            var result = await _receptionistService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Receptionist not found." });
            return Ok(new { message = "Receptionist deleted successfully." });
        }

        #endregion

        #region Staff Management

        [HttpGet("staff")]
        public async Task<IActionResult> GetAllStaff()
        {
            var staff = await _staffService.GetAllAsync();
            return Ok(staff);
        }

        [HttpPost("staff")]
        public async Task<IActionResult> CreateStaff(
            [FromBody] StaffRequestDTO dto,
            [FromServices] IValidator<StaffRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _staffService.CreateAsync(dto);
            return Ok(new { message = "Staff member created successfully.", staff = result });
        }

        [HttpDelete("staff/{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var result = await _staffService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Staff member not found." });
            return Ok(new { message = "Staff member deleted successfully." });
        }

        #endregion

        #region Patient Management

        [HttpGet("patients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllAsync();
            return Ok(patients);
        }

        [HttpGet("patients/{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found." });
            return Ok(patient);
        }

        [HttpDelete("patients/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Patient not found." });
            return Ok(new { message = "Patient deleted successfully." });
        }

        #endregion
    }
}
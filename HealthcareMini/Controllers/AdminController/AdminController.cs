// Controllers/Admin/AdminController.cs
using FluentValidation;
using HealthcareMini.DTOs.Admin;
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
   [Authorize(Roles = "Admin")]
   
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;
        private readonly DoctorService _doctorService;
        private readonly ReceptionistService _receptionistService;
        private readonly StaffService _staffService;
        private readonly PatientService _patientService;
        private readonly HealthCareCenterBaseService _centerCoreService;
        private readonly HealthCareCenterQueryService _centerQueryService;

        public AdminController(
            AdminService adminService,
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

        // POST: api/admin/admins
        // Create a new admin - only existing admins can create new admins
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(
            [FromBody] AdminRequestDTO dto,
            [FromServices] IValidator<AdminRequestDTO> validator)
        {
            // Validate the incoming DTO using FluentValidation
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            // Check if admin with same email already exists
            var existingAdmin = await _adminService.GetByEmailAsync(dto.Email);
            if (existingAdmin != null)
                return BadRequest(new { message = "An admin with this email already exists." });

            var result = await _adminService.CreateAsync(dto);
            return Ok(new { message = "Admin created successfully.", admin = result });
        }

        // GET: api/admin/admins
        // Get all admins - only admins can view other admins
        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAsync();
            return Ok(admins);
        }

        // GET: api/admin/admins/{id}
        // Get admin by ID - only admins can view admin details
        [HttpGet("admins/{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _adminService.GetByIdAsync(id);
            if (admin == null)
                return NotFound(new { message = "Admin not found." });
            return Ok(admin);
        }

        // GET: api/admin/centers
        // Get all health care centers - only admins can access this endpoint
        [HttpGet("centers")]
        public async Task<IActionResult> GetAllCenters()
        {
            var centers = await _centerQueryService.GetAllAsync();
            return Ok(centers);
        }

        // GET: api/admin/centers/{id}
        // Get health care center by ID - only admins can access this endpoint
        [HttpGet("centers/{id}")]
        public async Task<IActionResult> GetCenterById(int id)
        {
            var center = await _centerQueryService.GetByIdAsync(id);
            if (center == null)
                return NotFound(new { message = "Health care center not found." });
            return Ok(center);
        }

        // POST: api/admin/centers
        // Create a new health care center - only admins can create centers
        [HttpPost("centers")]
        public async Task<IActionResult> CreateCenter(
            [FromBody] CreateCenterDTO dto,
            [FromServices] IValidator<CreateCenterDTO> validator)
        {
            // Validate the incoming DTO using FluentValidation
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _centerCoreService.CreateAsync(dto);
            return Ok(new { message = "Health care center created successfully.", center = result });
        }

        // PUT: api/admin/centers/{id}
        // Update health care center information - only admins can edit any center
        [HttpPut("centers/{id}")]
        public async Task<IActionResult> UpdateCenter(
            int id,
            [FromBody] EditCenterDTO dto,
            [FromServices] IValidator<EditCenterDTO> validator)
        {
            // Validate the incoming DTO using FluentValidation
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _centerCoreService.EditAsync(id, dto);
            if (result == null)
                return NotFound(new { message = "Health care center not found." });
            return Ok(new { message = "Health care center updated successfully.", center = result });
        }

        // DELETE: api/admin/centers/{id}
        // Delete health care center - only admins can delete centers
        [HttpDelete("centers/{id}")]
        public async Task<IActionResult> DeleteCenter(int id)
        {
            var result = await _centerCoreService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Health care center not found." });
            return Ok(new { message = "Health care center deleted successfully." });
        }

        // POST: api/admin/centers/{id}/activate
        // Activate a health care center - only admins can activate centers
        [HttpPost("centers/{id}/activate")]
        public async Task<IActionResult> ActivateCenter(int id)
        {
            var result = await _centerCoreService.ActivateAsync(id);
            if (!result)
                return NotFound(new { message = "Health care center not found." });
            return Ok(new { message = "Health care center activated successfully." });
        }

        // POST: api/admin/centers/{id}/deactivate
        // Deactivate a health care center - only admins can deactivate centers
        [HttpPost("centers/{id}/deactivate")]
        public async Task<IActionResult> DeactivateCenter(int id)
        {
            var result = await _centerCoreService.DeactivateAsync(id);
            if (!result)
                return NotFound(new { message = "Health care center not found." });
            return Ok(new { message = "Health care center deactivated successfully." });
        }

        // GET: api/admin/doctors
        // Get all doctors - only admins can view all doctors
        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);
        }

        // GET: api/admin/doctors/{id}
        // Get doctor by ID - only admins can view any doctor
        [HttpGet("doctors/{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found." });
            return Ok(doctor);
        }

        // POST: api/admin/doctors
        // Create a new doctor - only admins can create doctors
        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor(
            [FromBody] DoctorRequestDTO dto,
            [FromServices] IValidator<DoctorRequestDTO> validator)
        {
            // Validate the incoming DTO using FluentValidation
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _doctorService.CreateAsync(dto);
            return Ok(new { message = "Doctor created successfully.", doctor = result });
        }

        // PUT: api/admin/doctors/{id}
        // Update doctor information - only admins can edit any doctor
        [HttpPut("doctors/{id}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            [FromBody] DoctorRequestDTO dto,
            [FromServices] IValidator<DoctorRequestDTO> validator)
        {
            // Validate the incoming DTO using FluentValidation
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _doctorService.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(new { message = "Doctor not found." });
            return Ok(new { message = "Doctor updated successfully.", doctor = result });
        }

        // DELETE: api/admin/doctors/{id}
        // Delete doctor - only admins can delete doctors
        [HttpDelete("doctors/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var result = await _doctorService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Doctor not found." });
            return Ok(new { message = "Doctor deleted successfully." });
        }

        // GET: api/admin/receptionists
        // Get all receptionists - only admins can view all receptionists
        [HttpGet("receptionists")]
        public async Task<IActionResult> GetAllReceptionists()
        {
            var receptionists = await _receptionistService.GetAllAsync();
            return Ok(receptionists);
        }

        // POST: api/admin/receptionists
        // Create a new receptionist - only admins can create receptionists
        [HttpPost("receptionists")]
        public async Task<IActionResult> CreateReceptionist(
            [FromBody] ReceptionistRequestDTO dto,
            [FromServices] IValidator<ReceptionistRequestDTO> validator)
        {
            // Validate the incoming DTO using FluentValidation
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _receptionistService.CreateAsync(dto);
            return Ok(new { message = "Receptionist created successfully.", receptionist = result });
        }

        // DELETE: api/admin/receptionists/{id}
        // Delete receptionist - only admins can delete receptionists
        [HttpDelete("receptionists/{id}")]
        public async Task<IActionResult> DeleteReceptionist(int id)
        {
            var result = await _receptionistService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Receptionist not found." });
            return Ok(new { message = "Receptionist deleted successfully." });
        }

        // GET: api/admin/staff
        // Get all staff members - only admins can view all staff
        [HttpGet("staff")]
        public async Task<IActionResult> GetAllStaff()
        {
            var staff = await _staffService.GetAllAsync();
            return Ok(staff);
        }

        // POST: api/admin/staff
        // Create a new staff member - only admins can create staff
        [HttpPost("staff")]
        public async Task<IActionResult> CreateStaff(
            [FromBody] StaffRequestDTO dto,
            [FromServices] IValidator<StaffRequestDTO> validator)
        {
            // Validate the incoming DTO using FluentValidation
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _staffService.CreateAsync(dto);
            return Ok(new { message = "Staff member created successfully.", staff = result });
        }

        // DELETE: api/admin/staff/{id}
        // Delete staff member - only admins can delete staff
        [HttpDelete("staff/{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var result = await _staffService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Staff member not found." });
            return Ok(new { message = "Staff member deleted successfully." });
        }

        // GET: api/admin/patients
        // Get all patients - only admins can view all patients
        [HttpGet("patients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllAsync();
            return Ok(patients);
        }

        // DELETE: api/admin/patients/{id}
        // Delete patient - only admins can delete patients
        [HttpDelete("patients/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Patient not found." });
            return Ok(new { message = "Patient deleted successfully." });
        }
    }
}
using FluentValidation;
using HealthcareMini.DTOs.Patient;
using HealthcareMini.Services.PatientServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareMini.Controllers.PatientsContoller
{
    [Route("api/patient/profile")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientProfileController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientProfileController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/patient/profile/me - Get current patient's own profile
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var patient = await _patientService.GetByIdAsync(userId);

            if (patient == null)
                return NotFound(new { message = "Your profile was not found." });

            return Ok(patient);
        }

        // PUT: api/patient/profile/me - Update current patient's own profile
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] PatientRequestDTO dto,
            [FromServices] IValidator<PatientRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var existing = await _patientService.GetByIdAsync(userId);
            if (existing == null)
                return NotFound(new { message = "Profile not found." });

            var fullDto = new PatientRequestDTO
            {
                FirstName = dto.FirstName ?? existing.FirstName,
                LastName = dto.LastName ?? existing.LastName,
                Email = dto.Email ?? existing.Email,
                PasswordHash = dto.PasswordHash ?? existing.PasswordHash,
                
                ContactDetails = dto.ContactDetails ?? existing.ContactDetails,
                AddressDetails = dto.AddressDetails ?? existing.AddressDetails
            };

            var updated = await _patientService.UpdateAsync(userId, fullDto);
            return Ok(new { message = "Profile updated successfully.", patient = updated });
        }

        // GET: api/patient/profile/appointments - Get current patient's appointments
        [HttpGet("appointments")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var appointments = await _patientService.GetPatientAppointmentsAsync(userId);
            return Ok(appointments);
        }

        // GET: api/patient/profile/medical-records - Get current patient's medical records
        [HttpGet("medical-records")]
        public async Task<IActionResult> GetMyMedicalRecords()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var records = await _patientService.GetPatientMedicalRecordsAsync(userId);
            return Ok(records);
        }
    }
}

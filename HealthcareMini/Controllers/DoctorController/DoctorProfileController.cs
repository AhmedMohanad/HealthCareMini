using FluentValidation;
using HealthcareMini.DTOs.Doctor;
using HealthcareMini.Services.DoctorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareMini.Controllers.DoctorController
{
   [Route("api/doctor/profile")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class DoctorProfileController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorProfileController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET: api/doctor/profile/me - Get current doctor's own profile
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var doctor = await _doctorService.GetByIdAsync(userId);
            
            if (doctor == null)
                return NotFound(new { message = "Your profile was not found." });
            
            return Ok(doctor);
        }

        // PUT: api/doctor/profile/me - Update current doctor's own profile
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] DoctorRequestDTO dto,
            [FromServices] IValidator<DoctorRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            var existing = await _doctorService.GetByIdAsync(userId);
            if (existing == null)
                return NotFound(new { message = "Profile not found." });

            var fullDto = new DoctorRequestDTO
            {
                FirstName = dto.FirstName ?? existing.FirstName,
                LastName = dto.LastName ?? existing.LastName,
                Email = dto.Email ?? existing.Email,
                Password = dto.Password ?? existing.PasswordHash,
                ContactDetails = dto.ContactDetails ?? existing.ContactDetails,
                AddressDetails = dto.AddressDetails ?? existing.AddressDetails,
                Salary = existing.Salary,
                Specialization = dto.Specialization ?? existing.Specialization,
                IsActive = existing.IsActive,
                HealthCareCenterIds = existing.HealthCareCenters.Select(c => c.Id).ToList()
            };

            var updated = await _doctorService.UpdateAsync(userId, fullDto);
            return Ok(new { message = "Profile updated successfully.", doctor = updated });
        }

        // GET: api/doctor/profile/appointments - Get current doctor's appointments
        [HttpGet("appointments")]
        public async Task<IActionResult> GetMyAppointments([FromQuery] DateTime? date = null)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var appointments = await _doctorService.GetDoctorAppointmentsAsync(userId, date);
            return Ok(appointments);
        }
    }
}

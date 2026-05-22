using FluentValidation;
using HealthcareMini.DTOs.Staff;
using HealthcareMini.Services.StaffServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareMini.Controllers.StaffController
{
    [Route("api/staff/profile")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class StaffProfileController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffProfileController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        // GET: api/staff/profile/me - Get current staff member's own profile
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var staff = await _staffService.GetByIdAsync(userId);

            if (staff == null)
                return NotFound(new { message = "Your profile was not found." });

            return Ok(staff);
        }

        // PUT: api/staff/profile/me - Update current staff member's own profile
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] UpdateStaffProfileDTO dto,
            [FromServices] IValidator<UpdateStaffProfileDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var existing = await _staffService.GetByIdAsync(userId);
            if (existing == null)
                return NotFound(new { message = "Profile not found." });

            var fullDto = new StaffRequestDTO
            {
                FirstName = dto.FirstName ?? existing.FirstName,
                LastName = dto.LastName ?? existing.LastName,
                Email = dto.Email ?? existing.Email,
                PasswordHash = dto.Password ?? existing.PasswordHash,
                DateOfBirth = dto.DateOfBirth ?? existing.DateOfBirth,
                ContactDetails = dto.ContactDetails ?? existing.ContactDetails,
                AddressDetails = dto.AddressDetails ?? existing.AddressDetails,
                Salary = existing.Salary,
                JobTitle = dto.JobTitle ?? existing.JobTitle,
                HealthCareCenterIds = existing.HealthCareCenters.Select(c => c.Id).ToList()
            };

            var updated = await _staffService.UpdateAsync(userId, fullDto);
            return Ok(new { message = "Profile updated successfully.", staff = updated });
        }
    }
}

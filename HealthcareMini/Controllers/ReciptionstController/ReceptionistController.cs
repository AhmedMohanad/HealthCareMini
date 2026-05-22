using FluentValidation;
using HealthcareMini.DTOs.Receptionist;
using HealthcareMini.Services.PasswordServices;
using HealthcareMini.Services.ReceptionistServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareMini.Controllers.ReciptionstController
{
    [Route("api/receptionist/profile")]
    [ApiController]
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistProfileController : ControllerBase
    {
        private readonly IReceptionistService _receptionistService;
        private readonly IPasswordService _passwordService;

        public ReceptionistProfileController(IReceptionistService receptionistService, IPasswordService passwordService)
        {
            _receptionistService = receptionistService;
            _passwordService = passwordService;
        }

        // GET: api/receptionist/profile/me - Get current receptionist's own profile
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var receptionist = await _receptionistService.GetByIdAsync(userId);

            if (receptionist == null)
                return NotFound(new { message = "Your profile was not found." });

            return Ok(receptionist);
        }

        // PUT: api/receptionist/profile/me - Update current receptionist's own profile
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] ReceptionistRequestDTO dto,
            [FromServices] IValidator<ReceptionistRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var existing = await _receptionistService.GetByIdAsync(userId);
            if (existing == null)
                return NotFound(new { message = "Profile not found." });

            var fullDto = new ReceptionistRequestDTO
            {
                FirstName = dto.FirstName ?? existing.FirstName,
                LastName = dto.LastName ?? existing.LastName,
                Email = dto.Email ?? existing.Email,
                Password = _passwordService.HashPassword(dto.Password) ?? existing.PasswordHash,

                ContactDetails = dto.ContactDetails ?? existing.ContactDetails,
                AddressDetails = dto.AddressDetails ?? existing.AddressDetails,
                Salary = existing.Salary,
                HealthCareCenterIds = existing.HealthCareCenters.Select(c => c.Id).ToList()
            };

            var updated = await _receptionistService.UpdateAsync(userId, fullDto);
            return Ok(new { message = "Profile updated successfully.", receptionist = updated });
        }

    }
}

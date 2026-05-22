using FluentValidation;
using HealthcareMini.DTOs.Staff;
using HealthcareMini.Services.StaffServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareMini.Controllers.StaffController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        // GET: api/staff - Get all staff members (Admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var staff = await _staffService.GetAllAsync();
            return Ok(staff);
        }

        // GET: api/staff/{id} - Get staff by ID (Admin, Center, Receptionist, or the staff themselves)
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,HealthCareCenter,Receptionist,Staff")]
        public async Task<IActionResult> GetById(int id)
        {
            if (User.IsInRole("Staff"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId != id)
                    return Forbid("You can only view your own profile.");
            }

            var staff = await _staffService.GetByIdAsync(id);
            if (staff == null)
                return NotFound(new { message = "Staff member not found." });

            return Ok(staff);
        }

        // GET: api/staff/center/{centerId} - Get all staff at a specific center (Admin, Center, Receptionist, Staff)
        [HttpGet("center/{centerId:int}")]
        [Authorize(Roles = "Admin,HealthCareCenter,Receptionist,Staff")]
        public async Task<IActionResult> GetByCenter(int centerId)
        {
            var staff = await _staffService.GetStaffByCenterAsync(centerId);
            return Ok(staff);
        }

        // POST: api/staff - Create a new staff member (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [FromBody] StaffRequestDTO dto,
            [FromServices] IValidator<StaffRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var existing = await _staffService.GetByEmailAsync(dto.Email);
            if (existing != null)
                return Conflict(new { message = "A staff member with this email already exists." });

            var staff = await _staffService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = staff.Id },
                new { message = "Staff member created successfully.", staff });
        }

        // DELETE: api/staff/{id} - Delete a staff member (Admin only)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _staffService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Staff member not found." });

            return Ok(new { message = "Staff member deleted successfully." });
        }
    }
}

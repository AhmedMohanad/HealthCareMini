using FluentValidation;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Services.HealthCareCenterServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCareCentersController : ControllerBase
    {
        private readonly HealthCareCenterBaseService _coreService;
        private readonly HealthCareCenterQueryService _queryService;

        public HealthCareCentersController(
            HealthCareCenterBaseService coreService,
            HealthCareCenterQueryService queryService)
        {
            _coreService = coreService;
            _queryService = queryService;
        }

        // GET api/HealthCareCenters
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var centers = await _queryService.GetLimitedAsync();
            if (!centers.Any())
                return NotFound("No health care centers found.");
            return Ok(centers);
        }

        // GET api/HealthCareCenters/{id}
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,HealthCareCenter")]
        public async Task<IActionResult> GetById(int id)
        {
            var center = await _queryService.GetByIdAsync(id);
            return center is null
                ? NotFound("HealthCareCenter not found.")
                : Ok(center);
        }

        // GET api/HealthCareCenters/email/{email}
        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var center = await _queryService.GetByEmailAsync(email);
            return center is null
                ? NotFound("HealthCareCenter not found.")
                : Ok(center);
        }

        // GET api/HealthCareCenters/name/{name}
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var center = await _queryService.GetByNameAsync(name);
            return center is null
                ? NotFound("HealthCareCenter not found.")
                : Ok(center);
        }

        // GET api/HealthCareCenters/limited
        [AllowAnonymous]
        [HttpGet("limited")]
        public async Task<IActionResult> GetLimited()
        {
            var centers = await _queryService.GetLimitedAsync();
            if (!centers.Any())
                return NotFound("No health care centers found.");
            return Ok(centers);
        }

        // POST api/HealthCareCenters
        // [ValidateAntiForgeryToken] REMOVED — incompatible with JWT/cookie REST APIs.
        // AntiForgery tokens are a browser-form defence; this API uses JWT authentication.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [FromBody] CreateCenterDTO dto,
            [FromServices] IValidator<CreateCenterDTO> validator)
        {
            if (dto is null)
                return BadRequest("Invalid health care center data.");

            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var result = await _coreService.CreateAsync(dto);
            return Ok(new { message = "HealthCareCenter created successfully.", center = result });
        }

        // PUT api/HealthCareCenters/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,HealthCareCenter")]
        public async Task<IActionResult> Edit(
            int id,
            [FromBody] EditCenterDTO dto,
            [FromServices] IValidator<EditCenterDTO> validator)
        {
            if (dto is null)
                return BadRequest("Invalid health care center data.");

            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            // HealthCareCenter role can only edit its own record.
            if (User.IsInRole("HealthCareCenter"))
            {
                var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
                var userCenter = await _queryService.GetByEmailAsync(userEmail!);

                if (userCenter is null || userCenter.Id != id)
                    return Unauthorized("You are not authorized to edit this health care center.");
            }

            var result = await _coreService.EditAsync(id, dto);
            return result is null
                ? NotFound("HealthCareCenter not found.")
                : Ok(new { message = "HealthCareCenter updated successfully.", center = result });
        }

        // DELETE api/HealthCareCenters/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _coreService.DeleteAsync(id);
            return result
                ? Ok(new { message = "HealthCareCenter deleted successfully." })
                : NotFound("HealthCareCenter not found.");
        }

        // DELETE api/HealthCareCenters/email/{email}
        [HttpDelete("email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByEmail(string email)
        {
            var result = await _coreService.DeleteByEmailAsync(email);
            return result
                ? Ok(new { message = "HealthCareCenter deleted successfully." })
                : NotFound("HealthCareCenter not found.");
        }

        // GET api/HealthCareCenters/{id}/employees
        [HttpGet("{id:int}/employees")]
        [Authorize(Roles = "Admin,HealthCareCenter")]
        public async Task<IActionResult> GetCenterEmployees(int id)
        {
            if (User.IsInRole("HealthCareCenter"))
            {
                var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
                var userCenter = await _queryService.GetByEmailAsync(userEmail!);

                if (userCenter is null || userCenter.Id != id)
                    return Unauthorized("You are not authorized to view employees of this center.");
            }

            var employees = await _queryService.GetEmployeesAsync(id);
            return Ok(employees);
        }

        // POST api/HealthCareCenters/{id}/activate
        [HttpPost("{id:int}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _coreService.ActivateAsync(id);
            return result
                ? Ok(new { message = "HealthCareCenter activated successfully." })
                : NotFound(new { message = "HealthCareCenter not found." });
        }

        // POST api/HealthCareCenters/{id}/deactivate
        [HttpPost("{id:int}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _coreService.DeactivateAsync(id);
            return result
                ? Ok(new { message = "HealthCareCenter deactivated successfully." })
                : NotFound(new { message = "HealthCareCenter not found." });
        }
    }
}
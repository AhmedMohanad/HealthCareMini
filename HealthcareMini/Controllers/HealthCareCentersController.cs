using FluentValidation;
using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Migrations;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Services.HealthCareCenterServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

[Route("api/[controller]")]
[ApiController]
public class HealthCareCentersController : ControllerBase
{
    private readonly HealthcareDbContext _context;
    private IHealthCareCenterServices _services;

    public HealthCareCentersController(HealthcareDbContext context, IHealthCareCenterServices services)
    {
        _context = context;
        _services = services;
    }


    // GET: api/HealthCareCenters
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        
            var centers = _services.GetAllAsync().Result; // Using .Result to get the result of the async method

            if (centers == null || !centers.Any())
            return NotFound("No health care centers found.");
        return Ok(centers);

    }

    // GET: api/HealthCareCenters/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var center = await _services.GetByIdAsync(id);
        if (center == null)
            return NotFound("HealthCareCenter not found.");
        return Ok(center);
    }

    // GET: api/HealthCareCenters/email/{email}
    [HttpGet("email/{email}")]
    [Authorize]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var center = await _services.GetByEmailAsync(email);
        if (center == null)
            return NotFound("HealthCareCenter not found.");
        return Ok(center);
    }

    // GET: api/HealthCareCenters/Centers
    // get a health care centers in the system for public users without the related data like doctors and receptionists and staff and appointments to improve performance when we only need basic information about the centers
    [AllowAnonymous]
    [HttpGet("Centers")]
    public async Task<IActionResult> GetLimited()
    {
        var centers = await _services.GetLimitedAsync();
        if (centers == null || !centers.Any())
            return NotFound("No health care centers found.");
        return Ok(centers);
    }


    // POST: api/HealthCareCenters
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize (Roles = "Admin")] 
    public async Task<IActionResult> Create([Bind("PasswordHash,Email,Name,IsActive,ContactDetails,AddressDetails,Doctors,Receptionists,Staff,Appointments")]CreateCenterDTO dto)
    {
        if (dto == null)
            return BadRequest("Invalid health care center data.");

        var result = await _services.CreateAsync(dto); 

        return result != null
            ? Ok("HealthCareCenter created successfully.")
            : BadRequest("Failed to create HealthCareCenter.");
    }


    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,HealthCareCenter")]
    public async Task<IActionResult> Edit(int id,
        [Bind("PasswordHash,Email,Name,IsActive,ContactDetails,AddressDetails,Doctors,Receptionists,Staff,Appointments")]
    EditCenterDTO dto,
       [FromServices] IValidator<EditCenterDTO> validator)
    {
        // Validate the incoming DTO using FluentValidation
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { errors });
        }


        // admins can edit any health care center information, but health care center users can only edit their own center information, so we need to check the user's role and id before allowing them to edit the center information
        // users with the role of health care center can only edit their own health care center information and not other centers information to prevent unauthorized access and maintain data integrity
        if (User.IsInRole("HealthCareCenter"))
        {
            var userId = int.Parse(User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId != id)
            {
                return Unauthorized("You are not authorized to edit this health care center.");
            }
        }

        var result = await _services.EditAsync(id, dto);

        return result != null
            ? Ok("HealthCareCenter updated successfully.")
            : BadRequest("Failed to update HealthCareCenter.");
    }


    // DELETE: api/HealthCareCenters/5
    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _services.DeleteAsync(id);
        return result
            ? Ok("HealthCareCenter deleted successfully.")
            : BadRequest("Failed to delete HealthCareCenter.");
    }

    //DELETE: api/HealthCareCenters/email/{email}
    [HttpDelete("email/{email}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteByEmail(string email)
    {
        bool result = await _services.DeleteByEmailAsync(email);
        return result
            ? Ok("HealthCareCenter deleted successfully.")
            : BadRequest("Failed to delete HealthCareCenter.");
    }


    [HttpGet("{id}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,HealthCareCenter")]
    public async Task<IActionResult> GetCenterEmployees(int id)
    {
        var userId = int.Parse(User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
        if (userId != id)
        {
            return Unauthorized("You are not authorized to this action.");
        }
        var employees = await _services.GetEmployeesAsync(id);
        return Ok(employees);
    }

    //GET: api/HealthCareCenters/name/{name}
    [HttpGet("name/{name}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByName(string name)
    {
        var center = await _services.GetByNameAsync(name);
        if (center == null)
            return NotFound("HealthCareCenter not found.");
        return Ok(center);
    }

}



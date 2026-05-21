using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Data;
using HealthcareMini.Services.HealthCareCenterServices;
using Microsoft.AspNetCore.Authorization;
using HealthcareMini.Migrations;
using HealthcareMini.DTOs.HealthCareCenterDTO;

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
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        
            var centers = _services.GetAllAsync().Result; // Using .Result to get the result of the async method


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



    
    
}
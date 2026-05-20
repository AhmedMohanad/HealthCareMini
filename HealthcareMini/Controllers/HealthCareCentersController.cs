using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Data;
using HealthcareMini.Services;

[Route("api/[controller]")]
[ApiController]
public class HealthCareCentersController : ControllerBase
{
    private readonly HealthcareDbContext _context;
    private HealthCareCenterServices _services;

    public HealthCareCentersController(HealthcareDbContext context, HealthCareCenterServices services)
    {
        _context = context;
        _services = new HealthCareCenterServices(context);
    }

    // GET: api/HealthCareCenters
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetAll()
    {
        
            var centers = await _context.HealthCareCenters.ToListAsync();
        
       
        return Ok(centers);
    }

    // GET: api/HealthCareCenters/5
    [HttpGet("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetById(int id)
    {
        var healthcarecenter = await _context.HealthCareCenters
            .FirstOrDefaultAsync(m => m.Id == id);

        if (healthcarecenter == null)
        {
            return NotFound(new { message = "HealthCareCenter not found" });
        }

        return Ok(healthcarecenter);
    }

    // POST: api/HealthCareCenters
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PasswordHash,Email,Name,IsActive,ContactDetails,AddressDetails,Doctors,Receptionists,Staff,Appointments")]HealthCareCenter healthcarecenter)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.HealthCareCenters.Add(healthcarecenter);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = healthcarecenter.Id },
            healthcarecenter
        );
    }

    // PUT: api/HealthCareCenters/5
    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PasswordHash,Email,Name,IsActive,ContactDetails,AddressDetails,Doctors,Receptionists,Staff,Appointments")]HealthCareCenter healthcarecenter)
    {
        if (id != healthcarecenter.Id)
        {
            return BadRequest(new { message = "ID mismatch" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _context.Update(healthcarecenter);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!HealthCareCenterExists(id))
            {
                return NotFound(new { message = "HealthCareCenter not found" });
            }

            throw;
        }

        return Ok(new { message = "Updated successfully" });
    }

    // DELETE: api/HealthCareCenters/5
    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var healthcarecenter = await _context.HealthCareCenters.FindAsync(id);

        if (healthcarecenter == null)
        {
            return NotFound(new { message = "HealthCareCenter not found" });
        }

        _context.HealthCareCenters.Remove(healthcarecenter);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Deleted successfully" });
    }

    private bool HealthCareCenterExists(int id)
    {
        return _context.HealthCareCenters.Any(e => e.Id == id);
    }
}
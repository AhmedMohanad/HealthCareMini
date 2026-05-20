using HealthcareMini.Controllers.CreationalPattrens.Factory;
using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.DTOs.LoginDTO;
using HealthcareMini.JWT;
using HealthcareMini.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly HealthcareDbContext _context;
    private AccountTypeFactory _accountTypeFactory;
    private HealthCareCenterServices _centerService;
    private JWT _jwt;

    public AuthController(IConfiguration configuration,
                          HealthcareDbContext context)
    {
        _configuration = configuration;
        _context = context;
        _jwt = new JWT(configuration);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO model)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (doctor != null)
        {
            // check password here
            var token = _jwt.GenerateJwtToken(doctor.Email, "Doctor");

            return Ok(new
            {
                token = token,
                role = "Doctor"
            });
        }

        var admin = await _context.Admins
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (admin != null)
        {
            var token = _jwt.GenerateJwtToken(admin.Email, "Admin");

            return Ok(new
            {
                token = token,
                role = "Admin"
            });
        }

        return Unauthorized();
    }



    [HttpPost("HealthCareCenterRegister")]
    public async Task<IActionResult> HealthCareCenterRegister([FromBody] CreateCenterDTO healthcareCenter)
    {
        _centerService = new HealthCareCenterServices(_context);

        
        var newHealthCareCenter = await _centerService.CreateAsync(healthcareCenter);

        if (newHealthCareCenter == null)
        {
            return BadRequest("Failed to register HealthCareCenter.");
        }

        var token = _jwt.GenerateJwtToken(newHealthCareCenter.Email, "HealthCareCenter");

        // Set HTTP-Only Cookie
        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,      // JavaScript cannot access
            Secure = true,        // HTTPS only (set false for local development)
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(1000),
            Path = "/",
            IsEssential = true    // GDPR compliance
        });

        return Ok(new
        {
            message = "HealthCareCenter registered successfully!" + newHealthCareCenter.Name + token
        });


    }
    
}

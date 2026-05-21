using HealthcareMini.Controllers.CreationalPattrens.Factory;
using HealthcareMini.Cookis;
using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.DTOs.LoginDTO;
using HealthcareMini.JWT;
using HealthcareMini.Services.HealthCareCenterServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly HealthcareDbContext _context;
    private readonly AccountTypeFactory _accountTypeFactory;
    private readonly IHealthCareCenterServices _centerService;
    private readonly IJwtService _jwt;
    private readonly ICookieService _cookieService;
    private readonly ILogger<AuthController> _logger;


    public AuthController(IConfiguration configuration,
                          HealthcareDbContext context,
                          IJwtService jwt,
                          ICookieService cookieService,
                          IHealthCareCenterServices centerService,
                          ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _context = context;
        _jwt = jwt;
        _cookieService = cookieService;
        _centerService = centerService;
        _logger = logger;
    }



    // this method is to register a health care center and it will be used by any on 

    [AllowAnonymous]
    [HttpPost("CenterRegister")]
    public async Task<IActionResult> HealthCareCenterRegister([FromBody] CreateCenterDTO healthcareCenter)
    {
       

        


        var newHealthCareCenter = await _centerService.CreateAsync(healthcareCenter);  // lazzy initialization of the service 

        // payment :)///
        // code of payment
        /////////////////

        if (newHealthCareCenter == null)
        {
            return BadRequest("Failed to register HealthCareCenter.");
        }

        // here the user will be registered and we will generate the token
        var token = _jwt.GenerateJwtToken(newHealthCareCenter.Id, newHealthCareCenter.Email, "HealthCareCenter");

        _logger.LogInformation("Register attempt started at {Time}", DateTime.UtcNow);
        // cookise options (user will have cookie and the token in this cookie)
        _cookieService.AppendAuthCookie(HttpContext.Response, token);
        await _centerService.ActivateAsync(newHealthCareCenter.Id); // activate the center after registration
       
        _logger.LogInformation("Center registered successfully. ID: {Id}, Name: {Name}",
       newHealthCareCenter.Id, newHealthCareCenter.Name);
        return Ok(new
        {
            message = newHealthCareCenter.Name + "Center  registered successfully!" 
        });


    }



    
    [HttpPost("logout")]
    [Authorize]  // User must be logged in to logout
    public async Task<IActionResult> Logout()
    {
        // Delete the cookie
        _cookieService.RemoveAuthCookie(HttpContext.Response);
        var userId = int.Parse(User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
       
        await _centerService.DeactivateAsync(userId); // Deactivate the center after logout
        _logger.LogInformation("User logged out. ID: {Id}", userId);
        return Ok(new
        {
            message = "Logged out successfully"
        });
    }

}

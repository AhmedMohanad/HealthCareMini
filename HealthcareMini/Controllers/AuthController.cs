using HealthcareMini.Controllers.CreationalPattrens.Factory;
using HealthcareMini.Cookis;
using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.DTOs.LoginDTO;
using HealthcareMini.JWT;
using HealthcareMini.Services;
using Microsoft.AspNetCore.Authorization;
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
    private JwtService _jwt;
    private ICookieService _cookieService;

    public AuthController(IConfiguration configuration,
                          HealthcareDbContext context,
                          JwtService jwt,
                          ICookieService cookieService)
    {
        _configuration = configuration;
        _context = context;
        _jwt = jwt;
        _cookieService = cookieService;
    }



    [AllowAnonymous]
    [HttpPost("CenterRegister")]
    public async Task<IActionResult> HealthCareCenterRegister([FromBody] CreateCenterDTO healthcareCenter)
    {
        _centerService = new HealthCareCenterServices(_context);

        


        var newHealthCareCenter = await _centerService.CreateAsync(healthcareCenter);  // lazzy initialization of the service 

        // payment :)///
        // code of payment
        /////////////////

        if (newHealthCareCenter == null)
        {
            return BadRequest("Failed to register HealthCareCenter.");
        }

        // here the user will be registered and we will generate the token
        var token = _jwt.GenerateJwtToken(newHealthCareCenter.Email, "HealthCareCenter");


        // cookise options (user will have cookie and the token in this cookie)
        _cookieService.AppendAuthCookie(HttpContext.Response, token);


        return Ok(new
        {
            message = newHealthCareCenter.Name + "Center  registered successfully!" 
        });


    }
    
}

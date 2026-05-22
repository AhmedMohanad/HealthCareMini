using FluentValidation;
using HealthcareMini.Controllers.CreationalPattrens.Factory;
using HealthcareMini.Cookis;
using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.DTOs.LoginDTO;
using HealthcareMini.JWT;
using HealthcareMini.Services.HealthCareCenterServices;
using HealthcareMini.Services.UserServices;
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
    private readonly IUserServices _userServices;


    public AuthController(IConfiguration configuration,
                          HealthcareDbContext context,
                          IJwtService jwt,
                          ICookieService cookieService,
                          IHealthCareCenterServices centerService,
                          ILogger<AuthController> logger,
                          IUserServices userServices)
    {
        _configuration = configuration;
        _context = context;
        _jwt = jwt;
        _cookieService = cookieService;
        _centerService = centerService;
        _logger = logger;
        _userServices = userServices;
    }



    // this method is to register a health care center and it will be used by any on 

    [AllowAnonymous]
    [HttpPost("CenterRegister")]
    public async Task<IActionResult> HealthCareCenterRegister([FromBody] CreateCenterDTO healthcareCenter,
                                                                [FromServices] IValidator<CreateCenterDTO> validator)
    {

        //validation of the data that the user sent to us (the data that we need to register the center)
        var validation = await validator.ValidateAsync(healthcareCenter);

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

    // this method is to login a health care center and it will be used by any one
    //POST: api/Auth/login
    [AllowAnonymous]
    [HttpPost("loginCenter")]
    public async Task<IActionResult> CenterLogin([FromBody] LoginDTO loginDTO, [FromServices] IValidator<LoginDTO> validator)
    {
        // validation of the data that the user sent to us (the data that we need to login the center)
        var validation = await validator.ValidateAsync(loginDTO);
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

        // Log the login attempt with email and timestamp
        _logger.LogWarning("login try", loginDTO.Email, DateTime.UtcNow);
        // Find the health care center by email
        var center = await _context.HealthCareCenters
            .FirstOrDefaultAsync(c => c.Email == loginDTO.Email);
        // Check if the center exists and the password is correct
        if (center == null || ! (loginDTO.Password == center.PasswordHash))
        {
            _logger.LogWarning("Failed login attempt for email: {Email} at {Time}", loginDTO.Email, DateTime.UtcNow);

            return Unauthorized(new{  message = "Invalid email or password"  });
        }

        // Generate JWT token and set cookie
        var token = _jwt.GenerateJwtToken(center.Id, center.Email, "HealthCareCenter");
        _cookieService.AppendAuthCookie(HttpContext.Response, token);

        await _centerService.ActivateAsync(center.Id); // Activate the center after successful login
        // Log the successful login with center ID and email
        _logger.LogInformation("User logged in successfully. ID: {Id}, Email: {Email}", center.Id, center.Email);
        return Ok(new
        {
            message = "Logged in successfully"
        });
    }


    // this method is to login a User and it will be used by any one
    [AllowAnonymous]
    [HttpPost("loginUser")]
    public async Task<IActionResult> UserLogin([FromBody] LoginDTO loginDTO, [FromServices] IValidator<LoginDTO> validator)
    {
        // validation of the data that the user sent to us (the data that we need to login the center)
        var validation = await validator.ValidateAsync(loginDTO);
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

        // Log the login attempt with email and timestamp
        _logger.LogWarning("login try", loginDTO.Email, DateTime.UtcNow);
        // Find the health care center by email
        var user = await _userServices.GetUserByEmailAsync(loginDTO.Email);
          
        if (user == null || !(loginDTO.Password == user.PasswordHash))
        {
            _logger.LogWarning("Failed login attempt for email: {Email} at {Time}", loginDTO.Email, DateTime.UtcNow);

            return Unauthorized(new { message = "Invalid email or password" });
        }

        // Generate JWT token and set cookie
        var token = _jwt.GenerateJwtToken(user.Id, user.Email, user.Role.ToString());
        _cookieService.AppendAuthCookie(HttpContext.Response, token);

       
        // Log the successful login with user ID and email
        _logger.LogInformation("User logged in successfully. ID: {Id}, Email: {Email}", user.Id, user.Email);
        return Ok(new
        {
            message = "Logged in successfully"
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

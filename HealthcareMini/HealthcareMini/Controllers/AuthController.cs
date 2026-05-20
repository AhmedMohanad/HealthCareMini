using HealthcareMini.Controllers.CreationalPattrens.Factory;
using HealthcareMini.Data;
using HealthcareMini.DTOs;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly HealthcareDbContext _context;
    private AccountTypeFactory _accountTypeFactory;

    public AuthController(IConfiguration configuration,
                          HealthcareDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO model)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (doctor != null)
        {
            // check password here
            var token = GenerateJwtToken(doctor.Email, "Doctor");

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
            var token = GenerateJwtToken(admin.Email, "Admin");

            return Ok(new
            {
                token = token,
                role = "Admin"
            });
        }

        return Unauthorized();
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Invalid registration data.");
        }

        try
        {
            // 1. Instantiate the factory and hand over the database context
            var accountTypeFactory = new AccountTypeFactory(_context);

            // 2. Let the factory determine the type and create it
            await accountTypeFactory.CreateUserAsync(dto);

            return Ok(new
            {
                message = $"{dto.Role} registered successfully!"
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during registration.", details = ex.Message });
        }
    }

    private string GenerateJwtToken(string email, string role)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, email),
        new Claim(ClaimTypes.Role, role)
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

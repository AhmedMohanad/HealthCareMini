using FluentValidation;
using HealthcareMini.Cookis;
using HealthcareMini.Data;
using HealthcareMini.JWT;
using HealthcareMini.Services.AdminServices;
using HealthcareMini.Services.AppointmentServices;
using HealthcareMini.Services.DoctorServices;
using HealthcareMini.Services.HealthCareCenterServices;
using HealthcareMini.Services.MedicalRecordServices;
using HealthcareMini.Services.PatientServices;
using HealthcareMini.Services.ReceptionistServices;
using HealthcareMini.Services.StaffServices;
using HealthcareMini.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ─── MVC / API ──────────────────────────────────────────────────────────────
// Use AddControllers() for a REST API — AddControllersWithViews() is for MVC
// with Razor views, which this project does not use.
builder.Services.AddControllers();

// ─── Database ────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<HealthcareDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ─── Settings ────────────────────────────────────────────────────────────────
builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection("CookieSettings"));

// ─── Infrastructure services ─────────────────────────────────────────────────
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICookieService, CookieService>();

// ─── HealthCareCenter services ───────────────────────────────────────────────
// Register each sub-service individually so they can be injected into both the
// facade AND the controllers that use them directly.
builder.Services.AddScoped<IHealthCareCenterBaseService, HealthCareCenterBaseService>();
builder.Services.AddScoped<IHealthCareCenterQueryService, HealthCareCenterQueryService>();
builder.Services.AddScoped<IHealthCareCenterEmployeeService, HealthCareCenterEmployeeService>();
builder.Services.AddScoped<IHealthCareCenterAppointmentService, HealthCareCenterAppointmentService>();
// Facade — depends on the four services above, which DI will inject.
builder.Services.AddScoped<IHealthCareCenterServices, HealthCareCenterServices>();

// ─── Domain services ─────────────────────────────────────────────────────────
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDoctorService,DoctorService>();
builder.Services.AddScoped<IReceptionistService, ReceptionistService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<AppointmentManagementService>();
builder.Services.AddScoped<IUserServices, UserServices>();

// ─── Validation ──────────────────────────────────────────────────────────────
// Registers ALL validators found in the assembly — no need to list them one by one.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// ─── JWT Authentication ───────────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Read token from the "AuthToken" cookie in addition to the
        // standard Authorization header.
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                    context.Token = token;
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// ─── Build ────────────────────────────────────────────────────────────────────
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
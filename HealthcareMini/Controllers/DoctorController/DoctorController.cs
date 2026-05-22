using FluentValidation;
using HealthcareMini.DTOs.Doctor;
using HealthcareMini.Services.DoctorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareMini.Controllers.DoctorController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET: api/doctor - Get all doctors (Admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);
        }

        // GET: api/doctor/{id} - Get doctor by ID (Admin, Center, Receptionist, Staff, or the doctor themselves)
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,HealthCareCenter,Receptionist,Staff,Doctor")]
        public async Task<IActionResult> GetById(int id)
        {
            if (User.IsInRole("Doctor"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId != id)
                    return Forbid("You can only view your own profile.");
            }

            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found." });

            return Ok(doctor);
        }

        // GET: api/doctor/email/{email} - Get doctor by email (Admin, Center, Receptionist, Staff)
        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin,HealthCareCenter,Receptionist,Staff")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var doctor = await _doctorService.GetByEmailAsync(email);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found." });

            return Ok(doctor);
        }

        // GET: api/doctor/center/{centerId} - Get all doctors at a specific center (Authenticated users)
        [HttpGet("center/{centerId:int}")]
        [Authorize]
        public async Task<IActionResult> GetByCenter(int centerId)
        {
            var doctors = await _doctorService.GetDoctorsByCenterAsync(centerId);
            return Ok(doctors);
        }

        // POST: api/doctor - Create a new doctor (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [FromBody] DoctorRequestDTO dto,
            [FromServices] IValidator<DoctorRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var existing = await _doctorService.GetByEmailAsync(dto.Email);
            if (existing != null)
                return Conflict(new { message = "A doctor with this email already exists." });

            var doctor = await _doctorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = doctor.Id },
                new { message = "Doctor created successfully.", doctor });
        }

        // PUT: api/doctor/{id} - Update an existing doctor (Admin only)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] DoctorRequestDTO dto,
            [FromServices] IValidator<DoctorRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var doctor = await _doctorService.UpdateAsync(id, dto);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found." });

            return Ok(new { message = "Doctor updated successfully.", doctor });
        }

        // DELETE: api/doctor/{id} - Delete a doctor (Admin only)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _doctorService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Doctor not found." });

            return Ok(new { message = "Doctor deleted successfully." });
        }

        // GET: api/doctor/{id}/appointments - Get doctor's appointments (Admin, Doctor themself, Receptionist, Staff)
        [HttpGet("{id:int}/appointments")]
        [Authorize(Roles = "Admin,Doctor,Receptionist,Staff")]
        public async Task<IActionResult> GetAppointments(int id, [FromQuery] DateTime? date = null)
        {
            if (User.IsInRole("Doctor"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId != id)
                    return Forbid("You can only view your own appointments.");
            }

            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found." });

            var appointments = await _doctorService.GetDoctorAppointmentsAsync(id, date);
            return Ok(appointments);
        }
    }
}

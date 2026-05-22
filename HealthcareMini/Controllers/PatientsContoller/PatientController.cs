using FluentValidation;
using HealthcareMini.DTOs.Patient;
using HealthcareMini.Services.PatientServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareMini.Controllers.PatientsContoller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/patient - Get all patients (Admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientService.GetAllAsync();
            return Ok(patients);
        }

        // GET: api/patient/{id} - Get patient by ID (Admin, Doctor, Receptionist, or the patient themselves)
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,Receptionist,Patient")]
        public async Task<IActionResult> GetById(int id)
        {
            if (User.IsInRole("Patient"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId != id)
                    return Forbid("You can only view your own profile.");
            }

            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found." });

            return Ok(patient);
        }

        // GET: api/patient/email/{email} - Get patient by email (Admin, Doctor, Receptionist)
        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var patient = await _patientService.GetByEmailAsync(email);
            if (patient == null)
                return NotFound(new { message = "Patient not found." });

            return Ok(patient);
        }

        // POST: api/patient - Create a new patient (Public registration)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(
            [FromBody] PatientRequestDTO dto,
            [FromServices] IValidator<PatientRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var existing = await _patientService.GetByEmailAsync(dto.Email);
            if (existing != null)
                return Conflict(new { message = "A patient with this email already exists." });

            var patient = await _patientService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = patient.Id },
                new { message = "Patient registered successfully.", patient });
        }

        // PUT: api/patient/{id} - Update patient (Admin only)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] PatientRequestDTO dto,
            [FromServices] IValidator<PatientRequestDTO> validator)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors });

            var patient = await _patientService.UpdateAsync(id, dto);
            if (patient == null)
                return NotFound(new { message = "Patient not found." });

            return Ok(new { message = "Patient updated successfully.", patient });
        }

        // DELETE: api/patient/{id} - Delete patient (Admin only)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Patient not found." });

            return Ok(new { message = "Patient deleted successfully." });
        }

        // GET: api/patient/{id}/appointments - Get patient's appointments (Admin, Doctor, Receptionist, or the patient themselves)
        [HttpGet("{id:int}/appointments")]
        [Authorize(Roles = "Admin,Doctor,Receptionist,Patient")]
        public async Task<IActionResult> GetAppointments(int id)
        {
            if (User.IsInRole("Patient"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId != id)
                    return Forbid("You can only view your own appointments.");
            }

            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found." });

            var appointments = await _patientService.GetPatientAppointmentsAsync(id);
            return Ok(appointments);
        }

        // GET: api/patient/{id}/medical-records - Get patient's medical records (Admin, Doctor, Receptionist, or the patient themselves)
        [HttpGet("{id:int}/medical-records")]
        [Authorize(Roles = "Admin,Doctor,Receptionist,Patient")]
        public async Task<IActionResult> GetMedicalRecords(int id)
        {
            if (User.IsInRole("Patient"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId != id)
                    return Forbid("You can only view your own medical records.");
            }

            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found." });

            var records = await _patientService.GetPatientMedicalRecordsAsync(id);
            return Ok(records);
        }
    }
}

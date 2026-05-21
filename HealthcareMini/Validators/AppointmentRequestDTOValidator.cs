// Validators/Appointment/AppointmentRequestDTOValidator.cs
using FluentValidation;
using HealthcareMini.DTOs.Appointment;

namespace HealthcareMini.Validators.Appointment
{
    public class AppointmentRequestDTOValidator : AbstractValidator<AppointmentRequestDTO>
    {
        public AppointmentRequestDTOValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("Patient ID is required");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor ID is required");

            RuleFor(x => x.HealthCareCenterId)
                .GreaterThan(0).WithMessage("Health care center ID is required");

            RuleFor(x => x.AppointmentDate)
                .NotEmpty().WithMessage("Appointment date is required")
                .Must(BeInFuture).WithMessage("Appointment date must be in the future");

            RuleFor(x => x.AppointmentEndTime)
                .NotEmpty().WithMessage("Appointment end time is required")
                .GreaterThan(x => x.AppointmentDate).WithMessage("End time must be after start time");

            RuleFor(x => x.ReasonForVisit)
                .MaximumLength(500).WithMessage("Reason for visit must not exceed 500 characters");
        }

        private bool BeInFuture(DateTime date)
        {
            return date > DateTime.Now;
        }
    }
}
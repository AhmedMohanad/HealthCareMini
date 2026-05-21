// Validators/Appointment/AppointmentRescheduleDTOValidator.cs
using FluentValidation;
using HealthcareMini.DTOs.Appointment;

namespace HealthcareMini.Validators.Appointment
{
    public class AppointmentRescheduleDTOValidator : AbstractValidator<AppointmentRescheduleDTO>
    {
        public AppointmentRescheduleDTOValidator()
        {
            RuleFor(x => x.NewDate)
                .NotEmpty().WithMessage("New date is required")
                .Must(BeInFuture).WithMessage("New date must be in the future");

            RuleFor(x => x.NewEndTime)
                .NotEmpty().WithMessage("New end time is required")
                .GreaterThan(x => x.NewDate).WithMessage("End time must be after start time");

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("Reason must not exceed 500 characters");
        }

        private bool BeInFuture(DateTime date)
        {
            return date > DateTime.Now;
        }
    }
}
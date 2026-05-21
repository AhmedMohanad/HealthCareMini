// Validators/Appointment/AppointmentStatusUpdateDTOValidator.cs
using FluentValidation;
using HealthcareMini.DTOs.Appointment;

namespace HealthcareMini.Validators.Appointment
{
    public class AppointmentStatusUpdateDTOValidator : AbstractValidator<AppointmentStatusUpdateDTO>
    {
        public AppointmentStatusUpdateDTOValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value");
        }
    }
}
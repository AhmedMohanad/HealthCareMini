// Validators/Doctor/DoctorRequestDTOValidator.cs
using FluentValidation;
using HealthcareMini.DTOs.Doctor;

namespace HealthcareMini.Validators.Doctor
{
    public class DoctorRequestDTOValidator : AbstractValidator<DoctorRequestDTO>
    {
        public DoctorRequestDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.Salary)
                .GreaterThan(0).WithMessage("Salary must be greater than 0");

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required")
                .MaximumLength(100).WithMessage("Specialization must not exceed 100 characters");

            RuleFor(x => x.HealthCareCenterIds)
                .NotEmpty().WithMessage("At least one health care center is required");

            When(x => x.ContactDetails != null, () =>
            {
                RuleFor(x => x.ContactDetails.PhoneNumbers)
                    .NotEmpty().WithMessage("At least one phone number is required");

                RuleForEach(x => x.ContactDetails.PhoneNumbers)
                    .Matches(@"^[\+0-9]{10,15}$").WithMessage("Invalid phone number format");
            });
        }
    }
}
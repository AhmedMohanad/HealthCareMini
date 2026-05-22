using FluentValidation;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Enums;

namespace HealthcareMini.Validators
{
    public class EditCenterDTOValidator : AbstractValidator<EditCenterDTO>
    {
        public EditCenterDTOValidator()
        {
            // Name validation (optional since it's edit)
            RuleFor(x => x.Name)
                .MinimumLength(3).WithMessage("Name must be at least 3 characters")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            // Email validation
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            // Password validation (only if provided)
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Password));

            // ContactDetails validation (only if provided)
            When(x => x.ContactDetails != null, () =>
            {
                RuleFor(x => x.ContactDetails.PhoneNumbers)
                    .Must(phones => phones == null || phones.Any())
                    .WithMessage("If phone numbers are provided, at least one is required");

                RuleForEach(x => x.ContactDetails.PhoneNumbers)
                    .Matches(@"^[\+0-9]{10,15}$").WithMessage("Invalid phone number format")
                    .When(x => x.ContactDetails?.PhoneNumbers != null);
            });

            // AddressDetails validation (only if provided)
            When(x => x.AddressDetails != null, () =>
            {
                RuleFor(x => x.AddressDetails.Street)
                    .MinimumLength(5).WithMessage("Street must be at least 5 characters")
                    .When(x => !string.IsNullOrWhiteSpace(x.AddressDetails?.Street));

                RuleFor(x => x.AddressDetails.City)
                    .MinimumLength(2).WithMessage("City must be at least 2 characters")
                    .When(x => !string.IsNullOrWhiteSpace(x.AddressDetails?.City));

                RuleFor(x => x.AddressDetails.Province)
                    .IsInEnum().WithMessage("Invalid province value")
                    .When(x => x.AddressDetails?.Province != null);

                RuleFor(x => x.AddressDetails.ZipCode)
                    .Matches(@"^\d{5}$").WithMessage("Zip code must be 5 digits")
                    .When(x => !string.IsNullOrWhiteSpace(x.AddressDetails?.ZipCode));
            });

            // Collections validation (optional)
            RuleFor(x => x.Doctors)
                .Must(doctors => doctors == null || doctors.Count <= 100)
                .WithMessage("Cannot add more than 100 doctors at once");

            RuleFor(x => x.Receptionists)
                .Must(receptionists => receptionists == null || receptionists.Count <= 50)
                .WithMessage("Cannot add more than 50 receptionists at once");

            RuleFor(x => x.Staff)
                .Must(staff => staff == null || staff.Count <= 200)
                .WithMessage("Cannot add more than 200 staff at once");
        }
    }
}
// Validators/CreateCenterDTOValidator.cs
using FluentValidation;
using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Objects;
using Microsoft.EntityFrameworkCore;

public class CreateCenterDTOValidator : AbstractValidator<CreateCenterDTO>
{
    private readonly HealthcareDbContext _context;
    public CreateCenterDTOValidator(HealthcareDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MustAsync(BeUniqueEmail).WithMessage("Email already exists");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");

        RuleFor(x => x.ContactDetails)
            .NotNull().WithMessage("Contact details are required")
            .SetValidator(new ContactDetailsValidator()!);

        RuleFor(x => x.AddressDetails)
            .NotNull().WithMessage("Address details are required")
            .SetValidator(new AddressDetailsValidator()!);
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Check if email already exists in database
        return !await _context.HealthCareCenters
            .AnyAsync(c => c.Email == email, cancellationToken);
    }
}

// Validators/ContactDetailsValidator.cs
public class ContactDetailsValidator : AbstractValidator<ContactDetails>
{
    public ContactDetailsValidator()
    {
        RuleFor(x => x.PhoneNumbers)
            .NotEmpty().WithMessage("At least one phone number is required");

        RuleForEach(x => x.PhoneNumbers)
            .NotEmpty().WithMessage("Phone number cannot be empty")
            .MinimumLength(10).WithMessage("Phone number must be at least 10 digits")
            .MaximumLength(15).WithMessage("Phone number too long")
            .Matches(@"^[\+0-9]+$").WithMessage("Phone number contains invalid characters");
    }
}

// Validators/AddressDetailsValidator.cs
public class AddressDetailsValidator : AbstractValidator<AddressDetails>
{
    public AddressDetailsValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required")
            .MinimumLength(5).WithMessage("Street must be at least 5 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MinimumLength(2).WithMessage("City must be at least 2 characters");

        RuleFor(x => x.Province)
            .IsInEnum().WithMessage("Invalid province value");

        RuleFor(x => x.ZipCode)
            .Matches(@"^\d{5}$").WithMessage("Zip code must be 5 digits");
    }
}
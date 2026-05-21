// Validators/MedicalRecord/MedicalRecordRequestDTOValidator.cs
using FluentValidation;
using HealthcareMini.DTOs.MedicalRecord;

namespace HealthcareMini.Validators.MedicalRecord
{
    public class MedicalRecordRequestDTOValidator : AbstractValidator<MedicalRecordRequestDTO>
    {
        public MedicalRecordRequestDTOValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor ID is required");

            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("Patient ID is required");

            RuleFor(x => x.HealthCareCenterId)
                .GreaterThan(0).WithMessage("Health care center ID is required");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters");
        }
    }
}
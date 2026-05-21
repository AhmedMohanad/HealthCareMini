using HealthcareMini.Data;
using HealthcareMini.DTOs.HealthCareCenterDTO;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Objects;


namespace HealthcareMini.Services
{
    public class HealthCareCenterServices
    {
        private readonly HealthcareDbContext _context;

        public HealthCareCenterServices(HealthcareDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCareCenter> CreateAsync(CreateCenterDTO healthcareCenter)
        {

            // create a new instance of HealthCareCenter and copy properties from the input healthcareCenter

            HealthCareCenter newHealthCareCenter ;

            try
            {


                 newHealthCareCenter = new HealthCareCenter
                {
                    Name = healthcareCenter.Name,
                    Email = healthcareCenter.Email,
                    PasswordHash = healthcareCenter.PasswordHash,
                    IsActive = false,
                    Role = UserRole.HealthCareCenter,

                     // any thing else can be null or empty or default value
                     ContactDetails = new ContactDetails
                    {
                        PhoneNumbers = healthcareCenter.ContactDetails?.PhoneNumbers ?? new List<string>()
                    },
                    AddressDetails = new AddressDetails
                    {
                        Street = healthcareCenter.AddressDetails?.Street ?? string.Empty,
                        City = healthcareCenter.AddressDetails?.City ?? string.Empty,
                        Province = healthcareCenter.AddressDetails?.Province ?? Province.Baghdad,
                        ZipCode = healthcareCenter.AddressDetails?.ZipCode ?? string.Empty
                    },
                    Doctors = healthcareCenter.Doctors ?? new List<Doctor>(),
                    Receptionists = healthcareCenter.Receptionists ?? new List<Receptionist>(),
                    Staff = healthcareCenter.Staff ?? new List<Staff>(),
                    Appointments = healthcareCenter.Appointments ?? new List<Appointment>()
                };
                _context.HealthCareCenters.Add(newHealthCareCenter);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, rethrow, or return a specific result)
                throw new Exception("An error occurred while creating the health care center.", ex);
            }



            return newHealthCareCenter;
        }
    }
}
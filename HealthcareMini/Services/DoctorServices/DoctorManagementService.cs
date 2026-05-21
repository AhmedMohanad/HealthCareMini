using HealthcareMini.Data;
using HealthcareMini.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    // Service for doctor management operations
    public class DoctorManagementService : HealthCareCenterBaseService
    {
        public DoctorManagementService(HealthcareDbContext context) : base(context)
        {
        }

        // Add a new doctor to a specific health care center
        public async Task<Doctor?> AddDoctorAsync(int centerId, Doctor doctor)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Doctors)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return null;

                // Check if doctor already works at this center
                if (center.Doctors.Any(d => d.Email == doctor.Email))
                    throw new Exception("A doctor with this email already works at this center.");

                // Check if doctor already exists in database
                var existingDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Email == doctor.Email);

                if (existingDoctor != null)
                {
                    // Doctor exists, just add the relationship
                    center.Doctors.Add(existingDoctor);
                }
                else
                {
                    // New doctor, add them to the center
                    center.Doctors.Add(doctor);
                }

                await _context.SaveChangesAsync();
                return doctor;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the doctor.", ex);
            }
        }

        // Add multiple doctors to a specific health care center
        public async Task<IEnumerable<Doctor>> AddDoctorsAsync(int centerId, IEnumerable<Doctor> doctors)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Doctors)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    throw new Exception($"Health care center with ID {centerId} not found.");

                var addedDoctors = new List<Doctor>();

                foreach (Doctor doctor in doctors)
                {
                    // Check if doctor already works at this center
                    if (!center.Doctors.Any(d => d.Email == doctor.Email))
                    {
                        // Check if doctor already exists in database
                        var existingDoctor = await _context.Doctors
                            .FirstOrDefaultAsync(d => d.Email == doctor.Email);

                        if (existingDoctor != null)
                        {
                            // Doctor exists, just add relationship
                            center.Doctors.Add(existingDoctor);
                            addedDoctors.Add(existingDoctor);
                        }
                        else
                        {
                            // New doctor, create and add relationship
                            center.Doctors.Add(doctor);
                            addedDoctors.Add(doctor);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return addedDoctors;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding doctors.", ex);
            }
        }

        // Remove a doctor from a specific health care center
        public async Task<bool> RemoveDoctorAsync(int centerId, int doctorId)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Doctors)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return false;

                var doctor = center.Doctors.FirstOrDefault(d => d.Id == doctorId);
                if (doctor == null)
                    return false;

                center.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the doctor.", ex);
            }
        }
    }
}
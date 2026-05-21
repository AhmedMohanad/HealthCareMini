using HealthcareMini.Data;
using HealthcareMini.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    public class HealthCareCenterEmployeeService : HealthCareCenterBaseService
    {
        public HealthCareCenterEmployeeService(HealthcareDbContext context) : base(context)
        {
        }

        // ========== DOCTOR MANAGEMENT ==========
        public async Task<Doctor?> AddDoctorAsync(int centerId, Doctor doctor)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Doctors)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return null;

                if (center.Doctors.Any(d => d.Email == doctor.Email))
                    throw new Exception("A doctor with this email already works at this center.");

                var existingDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Email == doctor.Email);

                if (existingDoctor != null)
                {
                    center.Doctors.Add(existingDoctor);
                    await _context.SaveChangesAsync();
                    return existingDoctor;
                }
                else
                {
                    center.Doctors.Add(doctor);
                    await _context.SaveChangesAsync();
                    return doctor;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the doctor.", ex);
            }
        }

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

                foreach (var doctor in doctors)
                {
                    if (!center.Doctors.Any(d => d.Email == doctor.Email))
                    {
                        var existingDoctor = await _context.Doctors
                            .FirstOrDefaultAsync(d => d.Email == doctor.Email);

                        if (existingDoctor != null)
                        {
                            center.Doctors.Add(existingDoctor);
                            addedDoctors.Add(existingDoctor);
                        }
                        else
                        {
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

        public async Task<bool> RemoveDoctorFromCenterAsync(int centerId, int doctorId)
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
                throw new Exception("An error occurred while removing the doctor from center.", ex);
            }
        }

        // ========== RECEPTIONIST MANAGEMENT ==========
        public async Task<Receptionist?> AddReceptionistAsync(int centerId, Receptionist receptionist)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Receptionists)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return null;

                if (center.Receptionists.Any(r => r.Email == receptionist.Email))
                    throw new Exception("A receptionist with this email already works at this center.");

                var existingReceptionist = await _context.Receptionists
                    .FirstOrDefaultAsync(r => r.Email == receptionist.Email);

                if (existingReceptionist != null)
                {
                    center.Receptionists.Add(existingReceptionist);
                    await _context.SaveChangesAsync();
                    return existingReceptionist;
                }
                else
                {
                    center.Receptionists.Add(receptionist);
                    await _context.SaveChangesAsync();
                    return receptionist;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the receptionist.", ex);
            }
        }

        public async Task<IEnumerable<Receptionist>> AddReceptionistsAsync(int centerId, IEnumerable<Receptionist> receptionists)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Receptionists)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    throw new Exception($"Health care center with ID {centerId} not found.");

                var addedReceptionists = new List<Receptionist>();

                foreach (var receptionist in receptionists)
                {
                    if (!center.Receptionists.Any(r => r.Email == receptionist.Email))
                    {
                        var existingReceptionist = await _context.Receptionists
                            .FirstOrDefaultAsync(r => r.Email == receptionist.Email);

                        if (existingReceptionist != null)
                        {
                            center.Receptionists.Add(existingReceptionist);
                            addedReceptionists.Add(existingReceptionist);
                        }
                        else
                        {
                            center.Receptionists.Add(receptionist);
                            addedReceptionists.Add(receptionist);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return addedReceptionists;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding receptionists.", ex);
            }
        }

        public async Task<bool> RemoveReceptionistFromCenterAsync(int centerId, int receptionistId)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Receptionists)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return false;

                var receptionist = center.Receptionists.FirstOrDefault(r => r.Id == receptionistId);
                if (receptionist == null)
                    return false;

                center.Receptionists.Remove(receptionist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the receptionist from center.", ex);
            }
        }

        // ========== STAFF MANAGEMENT ==========
        public async Task<Staff?> AddStaffAsync(int centerId, Staff staff)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Staff)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return null;

                if (center.Staff.Any(s => s.Email == staff.Email))
                    throw new Exception("A staff member with this email already works at this center.");

                var existingStaff = await _context.Staff
                    .FirstOrDefaultAsync(s => s.Email == staff.Email);

                if (existingStaff != null)
                {
                    center.Staff.Add(existingStaff);
                    await _context.SaveChangesAsync();
                    return existingStaff;
                }
                else
                {
                    center.Staff.Add(staff);
                    await _context.SaveChangesAsync();
                    return staff;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the staff member.", ex);
            }
        }

        public async Task<IEnumerable<Staff>> AddStaffAsync(int centerId, IEnumerable<Staff> staffList)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Staff)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    throw new Exception($"Health care center with ID {centerId} not found.");

                var addedStaff = new List<Staff>();

                foreach (var staff in staffList)
                {
                    if (!center.Staff.Any(s => s.Email == staff.Email))
                    {
                        var existingStaff = await _context.Staff
                            .FirstOrDefaultAsync(s => s.Email == staff.Email);

                        if (existingStaff != null)
                        {
                            center.Staff.Add(existingStaff);
                            addedStaff.Add(existingStaff);
                        }
                        else
                        {
                            center.Staff.Add(staff);
                            addedStaff.Add(staff);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return addedStaff;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding staff members.", ex);
            }
        }

        public async Task<bool> RemoveStaffFromCenterAsync(int centerId, int staffId)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Staff)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return false;

                var staff = center.Staff.FirstOrDefault(s => s.Id == staffId);
                if (staff == null)
                    return false;

                center.Staff.Remove(staff);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the staff member from center.", ex);
            }
        }
    }
}
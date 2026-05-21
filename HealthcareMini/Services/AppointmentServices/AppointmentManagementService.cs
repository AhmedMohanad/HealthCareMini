using HealthcareMini.Data;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.AppointmentServices
{
    public class AppointmentManagementService
    {
        private readonly HealthcareDbContext _context;

        public AppointmentManagementService(HealthcareDbContext context)
        {
            _context = context;
        }

        // Check-in patient (mark as arrived)
        public async Task<Appointment?> CheckInAsync(int appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null)
                    return null;

                if (appointment.Status != AppointmentStatus.Scheduled)
                    throw new Exception($"Cannot check-in appointment with status: {appointment.Status}");

                appointment.Status = AppointmentStatus.CheckedIn;
                appointment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking in the appointment.", ex);
            }
        }

        // Complete appointment (mark as finished)
        public async Task<Appointment?> CompleteAsync(int appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null)
                    return null;

                if (appointment.Status != AppointmentStatus.CheckedIn)
                    throw new Exception($"Cannot complete appointment with status: {appointment.Status}");

                appointment.Status = AppointmentStatus.Completed;
                appointment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while completing the appointment.", ex);
            }
        }

        // Cancel appointment
        public async Task<Appointment?> CancelAsync(int appointmentId, string? cancelReason = null)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null)
                    return null;

                if (appointment.Status == AppointmentStatus.Completed)
                    throw new Exception("Cannot cancel a completed appointment.");

                if (appointment.Status == AppointmentStatus.Canceled)
                    throw new Exception("Appointment is already canceled.");

                appointment.Status = AppointmentStatus.Canceled;
                appointment.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(cancelReason))
                    appointment.ReasonForVisit = cancelReason; // Or add CancelReason field

                await _context.SaveChangesAsync();

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while canceling the appointment.", ex);
            }
        }

        // Mark as no-show (patient didn't arrive)
        public async Task<Appointment?> MarkAsNoShowAsync(int appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null)
                    return null;

                if (appointment.Status != AppointmentStatus.Scheduled &&
                    appointment.Status != AppointmentStatus.CheckedIn)
                    throw new Exception($"Cannot mark appointment as no-show with status: {appointment.Status}");

                appointment.Status = AppointmentStatus.NoShow;
                appointment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while marking appointment as no-show.", ex);
            }
        }

        // Reschedule appointment
        public async Task<Appointment?> RescheduleAsync(int appointmentId, DateTime newDate, DateTime newEndTime)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null)
                    return null;

                if (appointment.Status == AppointmentStatus.Completed)
                    throw new Exception("Cannot reschedule a completed appointment.");

                if (appointment.Status == AppointmentStatus.Canceled)
                    throw new Exception("Cannot reschedule a canceled appointment.");

                // Check for conflicts with doctor's schedule
                var hasConflict = await _context.Appointments
                    .AnyAsync(a => a.DoctorId == appointment.DoctorId &&
                                   a.Id != appointmentId &&
                                   a.Status != AppointmentStatus.Canceled &&
                                   ((newDate >= a.AppointmentDate && newDate < a.AppointmentEndTime) ||
                                    (newEndTime > a.AppointmentDate && newEndTime <= a.AppointmentEndTime) ||
                                    (newDate <= a.AppointmentDate && newEndTime >= a.AppointmentEndTime)));

                if (hasConflict)
                    throw new Exception("The selected time slot conflicts with another appointment.");

                appointment.AppointmentDate = newDate;
                appointment.AppointmentEndTime = newEndTime;
                appointment.Status = AppointmentStatus.Scheduled; // Reset to scheduled
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while rescheduling the appointment.", ex);
            }
        }

        // Reschedule with reason
        public async Task<Appointment?> RescheduleWithReasonAsync(int appointmentId, DateTime newDate, DateTime newEndTime, string rescheduleReason)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null)
                    return null;

                if (appointment.Status == AppointmentStatus.Completed)
                    throw new Exception("Cannot reschedule a completed appointment.");

                // Check for conflicts
                var hasConflict = await CheckDoctorAvailabilityAsync(appointment.DoctorId, newDate, newEndTime, appointmentId);
                if (hasConflict)
                    throw new Exception("Doctor is not available at this time.");

                appointment.AppointmentDate = newDate;
                appointment.AppointmentEndTime = newEndTime;
                appointment.Status = AppointmentStatus.Scheduled;
                appointment.UpdatedAt = DateTime.UtcNow;
                appointment.ReasonForVisit = rescheduleReason ?? appointment.ReasonForVisit;

                await _context.SaveChangesAsync();

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while rescheduling the appointment.", ex);
            }
        }

        // Check doctor availability
        public async Task<bool> CheckDoctorAvailabilityAsync(int doctorId, DateTime startTime, DateTime endTime, int? excludeAppointmentId = null)
        {
            var query = _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                           a.Status != AppointmentStatus.Canceled &&
                           ((startTime >= a.AppointmentDate && startTime < a.AppointmentEndTime) ||
                            (endTime > a.AppointmentDate && endTime <= a.AppointmentEndTime) ||
                            (startTime <= a.AppointmentDate && endTime >= a.AppointmentEndTime)));

            if (excludeAppointmentId.HasValue)
                query = query.Where(a => a.Id != excludeAppointmentId.Value);

            return await query.AnyAsync();
        }

        // Get available time slots for a doctor
        public async Task<List<TimeSlot>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date, int durationMinutes = 30)
        {
            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                           a.AppointmentDate.Date == date.Date &&
                           a.Status != AppointmentStatus.Canceled)
                .ToListAsync();

            var availableSlots = new List<TimeSlot>();
            var workStart = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0); // 9 AM
            var workEnd = new DateTime(date.Year, date.Month, date.Day, 17, 0, 0);   // 5 PM

            var currentSlot = workStart;

            while (currentSlot < workEnd)
            {
                var slotEnd = currentSlot.AddMinutes(durationMinutes);
                var isBooked = appointments.Any(a =>
                    (currentSlot >= a.AppointmentDate && currentSlot < a.AppointmentEndTime) ||
                    (slotEnd > a.AppointmentDate && slotEnd <= a.AppointmentEndTime));

                if (!isBooked && slotEnd <= workEnd)
                {
                    availableSlots.Add(new TimeSlot
                    {
                        StartTime = currentSlot,
                        EndTime = slotEnd,
                        IsAvailable = true
                    });
                }

                currentSlot = slotEnd;
            }

            return availableSlots;
        }

        // Get appointment by ID with details
        public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.HealthCareCenter)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
        }

        // Get all appointments for a patient
        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.HealthCareCenter)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        // Get all appointments for a doctor
        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateTime? date = null)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId);

            if (date.HasValue)
                query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);

            return await query.OrderBy(a => a.AppointmentDate).ToListAsync();
        }

        // Get appointments by status
        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(int centerId, AppointmentStatus status)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.HealthCareCenterId == centerId && a.Status == status)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        // Auto-cancel expired no-shows (run as background job)
        public async Task<int> AutoCancelNoShowsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var expiredAppointments = await _context.Appointments
                .Where(a => a.Status == AppointmentStatus.Scheduled &&
                           a.AppointmentDate.Date < today)
                .ToListAsync();

            foreach (var appointment in expiredAppointments)
            {
                appointment.Status = AppointmentStatus.NoShow;
                appointment.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return expiredAppointments.Count;
        }
    }

    // Helper class for time slots
    public class TimeSlot
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
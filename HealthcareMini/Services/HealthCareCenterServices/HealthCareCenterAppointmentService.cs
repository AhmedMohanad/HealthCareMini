using HealthcareMini.Data;
using HealthcareMini.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    public class HealthCareCenterAppointmentService : HealthCareCenterBaseService
    {
        public HealthCareCenterAppointmentService(HealthcareDbContext context) : base(context)
        {
        }

        public async Task<Appointment?> AddAppointmentAsync(int centerId, Appointment appointment)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Appointments)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return null;

                var hasConflict = center.Appointments.Any(a =>
                    a.DoctorId == appointment.DoctorId &&
                    a.AppointmentDate == appointment.AppointmentDate);

                if (hasConflict)
                    throw new Exception("This time slot is already booked for this doctor.");

                appointment.HealthCareCenterId = centerId;
                center.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the appointment.", ex);
            }
        }

        public async Task<IEnumerable<Appointment>> AddAppointmentsAsync(int centerId, IEnumerable<Appointment> appointments)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Appointments)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    throw new Exception($"Health care center with ID {centerId} not found.");

                var addedAppointments = new List<Appointment>();

                foreach (var appointment in appointments)
                {
                    var hasConflict = center.Appointments.Any(a =>
                        a.DoctorId == appointment.DoctorId &&
                        a.AppointmentDate == appointment.AppointmentDate);

                    if (!hasConflict)
                    {
                        appointment.HealthCareCenterId = centerId;
                        center.Appointments.Add(appointment);
                        addedAppointments.Add(appointment);
                    }
                }

                await _context.SaveChangesAsync();
                return addedAppointments;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding appointments.", ex);
            }
        }

        public async Task<bool> RemoveAppointmentFromCenterAsync(int centerId, int appointmentId)
        {
            try
            {
                var center = await _context.HealthCareCenters
                    .Include(h => h.Appointments)
                    .FirstOrDefaultAsync(h => h.Id == centerId);

                if (center == null)
                    return false;

                var appointment = center.Appointments.FirstOrDefault(a => a.Id == appointmentId);
                if (appointment == null)
                    return false;

                center.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the appointment from center.", ex);
            }
        }
    }
}
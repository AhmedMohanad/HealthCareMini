using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Objects;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Data
{
    /// <summary>
    /// Single database entry-point for the entire application.
    ///
    /// Inheritance strategy: Table-Per-Hierarchy (TPH)
    ///   All User subclasses (Doctor, Patient, Receptionist, Staff, Admin)
    ///   share the "Users" table.  EF adds a "Discriminator" column
    ///   automatically so it knows which row belongs to which class.
    /// </summary>
    public class HealthcareDbContext(DbContextOptions<HealthcareDbContext> options) : DbContext(options)
    {
        // ── Core tables ───────────────────────────────────────────────────
        public DbSet<HealthCareCenter> HealthCareCenters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ── Email must be unique across all users ─────────────────────
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ── Owned objects: inlined into the Users table ───────────────
            //
            // ⚠️  NEVER call builder.Entity<ContactDetails>() or
            //     builder.Entity<AddressDetails>() separately — that
            //     registers them as standalone entities, which directly
            //     conflicts with OwnsOne and causes the runtime crash:
            //     "cannot be configured as owned because it has already
            //      been configured as a non-owned"
            //
            // All configuration for owned types must live INSIDE OwnsOne().
            //
            builder.Entity<User>().OwnsOne(u => u.ContactDetails, cd =>
            {
                // List<string> → persisted as a JSON array in one column
                // e.g.  ["07701234567","07709876543"]
                cd.Property(c => c.PhoneNumbers)
                  .HasColumnType("nvarchar(max)")
                  .HasColumnName("ContactDetails_PhoneNumbers");
            });

            builder.Entity<User>().OwnsOne(u => u.AddressDetails, ad =>
            {
                // Store Province enum as its name string ("Baghdad", "Basra"…)
                // so the DB column is human-readable
                ad.Property(a => a.Province)
                  .HasConversion<string>()
                  .HasColumnName("AddressDetails_Province");
            });

            // ── Appointment: prevent multiple cascade paths ───────────────
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.HealthCareCenter)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.HealthCareCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Store enums as readable strings in the DB
            builder.Entity<Appointment>()
                .Property(a => a.Status)
                .HasConversion<string>();

            // ── MedicalRecord relationships ───────────────────────────────
            builder.Entity<MedicalRecord>()
                .HasOne(m => m.Doctor)
                .WithMany()
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MedicalRecord>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MedicalRecord>()
                .HasOne(m => m.HealthCareCenter)
                .WithMany()
                .HasForeignKey(m => m.HealthCareCenterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
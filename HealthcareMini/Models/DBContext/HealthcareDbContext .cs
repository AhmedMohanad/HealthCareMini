using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Objects;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Data
{
    public class HealthcareDbContext(DbContextOptions<HealthcareDbContext> options) : DbContext(options)
    {
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

            // ── Email must be unique across ALL users and HealthCareCenters ──
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<HealthCareCenter>()
                .HasIndex(h => h.Email)
                .IsUnique();

            // ── Configure owned objects for User ─────────────────────────
            builder.Entity<User>().OwnsOne(u => u.ContactDetails, cd =>
            {
                cd.Property(c => c.PhoneNumbers)
                  .HasColumnType("nvarchar(max)")
                  .HasColumnName("ContactDetails_PhoneNumbers");
            });

            builder.Entity<User>().OwnsOne(u => u.AddressDetails, ad =>
            {
                ad.Property(a => a.Province)
                  .HasConversion<string>()
                  .HasColumnName("AddressDetails_Province");
            });

            // ── Configure owned objects for HealthCareCenter ──────────────
            builder.Entity<HealthCareCenter>().OwnsOne(h => h.ContactDetails, cd =>
            {
                cd.Property(c => c.PhoneNumbers)
                  .HasColumnType("nvarchar(max)")
                  .HasColumnName("ContactDetails_PhoneNumbers");

                
            });

            builder.Entity<HealthCareCenter>().OwnsOne(h => h.AddressDetails, ad =>
            {
                ad.Property(a => a.Province)
                  .HasConversion<string>()
                  .HasColumnName("AddressDetails_Province");
            });

            // ── Many-to-Many: HealthCareCenter ↔ Doctors ──────────────────
            builder.Entity<HealthCareCenter>()
                .HasMany(h => h.Doctors)
                .WithMany(d => d.HealthCareCenters)
                .UsingEntity(j => j.ToTable("HealthCareCenterDoctors"));

            // ── Many-to-Many: HealthCareCenter ↔ Receptionists ────────────
            builder.Entity<HealthCareCenter>()
                .HasMany(h => h.Receptionists)
                .WithMany(r => r.HealthCareCenters)
                .UsingEntity(j => j.ToTable("HealthCareCenterReceptionists"));

            // ── Many-to-Many: HealthCareCenter ↔ Staff ────────────────────
            builder.Entity<HealthCareCenter>()
                .HasMany(h => h.Staff)
                .WithMany(s => s.HealthCareCenters)
                .UsingEntity(j => j.ToTable("HealthCareCenterStaff"));

            // ── Appointment relationships ─────────────────────────────────
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                
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

            // Store enums as readable strings
            builder.Entity<Appointment>()
                .Property(a => a.Status)
                .HasConversion<string>();

            // ── MedicalRecord relationships ──────────────────────────────
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
                .WithMany(c => c.MedicalRecords)
                .HasForeignKey(m => m.HealthCareCenterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
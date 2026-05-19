using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace HealthcareMini.Models.DBContext

{
    public class HealthcareDbContext : DbContext
    {

        public HealthcareDbContext(DbContextOptions<HealthcareDbContext> options)
    : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthCareCenter> HealthCareCenters { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<AddressDetails> AddressDetails { get; set; }
        public DbSet<ContentDetails> ContentDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== Inheritance Configuration (TPH) ==========
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasDiscriminator<string>("UserType")
                .HasValue<Admin>("Admin")
                .HasValue<Doctor>("Doctor")
                .HasValue<Patient>("Patient")
                .HasValue<Receptionist>("Receptionist")
                .HasValue<Staff>("Staff");

            // ========== Primary Keys ==========
            modelBuilder.Entity<AddressDetails>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<ContentDetails>()
                .HasKey(c => c.Id);

            // ========== Value Conversions ==========
            // Convert List<string> to JSON string for PhoneNumber
            modelBuilder.Entity<ContentDetails>()
                .Property(c => c.PhoneNumber)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>()
                );

            // Convert Province enum to string for better readability
            modelBuilder.Entity<AddressDetails>()
                .Property(a => a.Province)
                .HasConversion<string>();

            // ========== Relationships ==========

            // Appointment - Patient
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - Doctor
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - HealthCareCenter
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.HealthCareCenter)
                .WithMany(h => h.Appointments)
                .HasForeignKey(a => a.HealthCareCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // MedicalRecord - Doctor
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Doctor)
                .WithMany()
                .HasForeignKey("DoctorId")
                .OnDelete(DeleteBehavior.Restrict);

            // MedicalRecord - Patient
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey("PatientId")
                .OnDelete(DeleteBehavior.Restrict);

            // MedicalRecord - HealthCareCenter
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.HealthCareCenter)
                .WithMany()
                .HasForeignKey("HealthCareCenterId")
                .OnDelete(DeleteBehavior.Restrict);

            // HealthCareCenter - Doctors (Many-to-Many)
            modelBuilder.Entity<HealthCareCenter>()
                .HasMany(h => h.Doctors)
                .WithMany()
                .UsingEntity(j => j.ToTable("HealthCareCenterDoctors"));

            // HealthCareCenter - Receptionists (Many-to-Many)
            modelBuilder.Entity<HealthCareCenter>()
                .HasMany(h => h.Receptionists)
                .WithMany()
                .UsingEntity(j => j.ToTable("HealthCareCenterReceptionists"));

            // HealthCareCenter - Staff (Many-to-Many)
            modelBuilder.Entity<HealthCareCenter>()
                .HasMany(h => h.Staff)
                .WithMany()
                .UsingEntity(j => j.ToTable("HealthCareCenterStaff"));

            // ========== Property Configurations ==========

            // Decimal precision for salaries
            modelBuilder.Entity<Doctor>()
                .Property(d => d.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Receptionist>()
                .Property(r => r.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Staff>()
                .Property(s => s.Salary)
                .HasPrecision(18, 2);

            // Indexes for better performance
            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.Specialization);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.AppointmentDate);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.DoctorId, a.AppointmentDate });

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.FirstName, u.LastName });

            modelBuilder.Entity<HealthCareCenter>()
                .HasIndex(h => h.Name)
                .IsUnique();

            // ContentDetails - Email unique constraint
            modelBuilder.Entity<ContentDetails>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // ========== Shadow Properties for Foreign Keys ==========
            modelBuilder.Entity<MedicalRecord>()
                .Property<int>("DoctorId");

            modelBuilder.Entity<MedicalRecord>()
                .Property<int>("PatientId");

            modelBuilder.Entity<MedicalRecord>()
                .Property<int>("HealthCareCenterId");

            // ========== Default Values ==========
            modelBuilder.Entity<User>()
                .Property(u => u.DateOfRegistration)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Appointment>()
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<MedicalRecord>()
                .Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}

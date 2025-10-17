using Microsoft.EntityFrameworkCore;
using appointment_backend.Models;

namespace MedicalSys.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

       
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.Document)
                .IsUnique();
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.Document)
                .IsUnique();
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<EmailLog>()
                .HasOne(e => e.Appointment)
                .WithMany()
                .HasForeignKey(e => e.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Speciality>().HasMany(s => s.DoctorSpecialities)
                .WithOne(ds => ds.Speciality)
                .HasForeignKey(ds => ds.SpecialityId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Doctor>().HasMany(s => s.DoctorSpecialities)
                .WithOne(ds => ds.Doctor)
                .HasForeignKey(ds => ds.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DoctorSpeciality>().HasOne(s => s.SubSpeciality)
                .WithMany(ss => ss.DoctorSpecialities)
                .HasForeignKey(s => s.SubSpecialityId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DocumentType>().HasMany(dt => dt.Doctors)
                .WithOne(d => d.DocumentType)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DocumentType>().HasMany(dt => dt.Patients)
                .WithOne(p => p.DocumentType)
                .HasForeignKey(p => p.DocumentTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DoctorSpeciality>()
                .HasKey(p => new { p.SpecialityId, p.DoctorId, p.SubSpecialityId });
            modelBuilder.Entity<Doctor>().HasKey(d => d.Id);
            modelBuilder.Entity<Patient>().HasKey(p => p.Id);
            modelBuilder.Entity<Appointment>().HasKey(a => a.Id);
            modelBuilder.Entity<EmailLog>().HasKey(a => a.Id);
            modelBuilder.Entity<Speciality>().HasKey(s => s.Id);
            modelBuilder.Entity<DoctorSpeciality>().HasKey(s => new { s.SpecialityId, s.DoctorId });
            modelBuilder.Entity<DocumentType>().HasKey(dt => dt.Id);
        }
    }
}
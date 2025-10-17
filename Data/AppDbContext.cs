using appointment_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace appointment_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        //Estas son todas las tablas que se crean
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<AppointmentStatus> AppointmentStatuses { get; set; }
        public DbSet<EmailStatus> EmailStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aqui se indica que los documentos son un dato unico en la base de datos
            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.Document)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.Document)
                .IsUnique();

            
            //Aqui van las relaciones para asegurar que EF las tome adecuadamente
            
            
            // Cita doctor
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            
            // Cita paciente
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            
            
            // Email a la cita
            modelBuilder.Entity<EmailLog>()
                .HasOne(e => e.Appointment)
                .WithMany()
                .HasForeignKey(e => e.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            
            // Doctor a especialidad
           modelBuilder.Entity<Doctor>()
               .HasOne(d => d.Speciality)
               .WithMany(s => s.Doctors)
               .HasForeignKey(d => d.SpecialityId)
               .OnDelete(DeleteBehavior.Restrict);
         
           
           // Cita a estatus de cita
           modelBuilder.Entity<Appointment>()
               .HasOne(a => a.Status)
               .WithMany(s => s.Appointments)
               .HasForeignKey(a => a.StatusId)
               .OnDelete(DeleteBehavior.Restrict);
           
           //Email a estatus de email
           modelBuilder.Entity<EmailLog>()
               .HasOne(e => e.Status)
               .WithMany(s => s.Emails)
               .HasForeignKey(e => e.StatusId)
               .OnDelete(DeleteBehavior.Restrict);
           
           
           // Documento a doctores
            modelBuilder.Entity<DocumentType>()
                .HasMany(dt => dt.Doctors)
                .WithOne(d => d.DocumentType)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

           //Documento a pacientes
            modelBuilder.Entity<DocumentType>()
                .HasMany(dt => dt.Patients)
                .WithOne(p => p.DocumentType)
                .HasForeignKey(p => p.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            
            
            
            //Aqui se determinan todas las llaves primarias
            
            modelBuilder.Entity<Doctor>().HasKey(d => d.Id);
            modelBuilder.Entity<Patient>().HasKey(p => p.Id);
            modelBuilder.Entity<Appointment>().HasKey(a => a.Id);
            modelBuilder.Entity<EmailLog>().HasKey(e => e.Id);
            modelBuilder.Entity<Speciality>().HasKey(s => s.Id);
            modelBuilder.Entity<DocumentType>().HasKey(dt => dt.Id);
            modelBuilder.Entity<AppointmentStatus>().HasKey(dt => dt.Id);
            modelBuilder.Entity<EmailStatus>().HasKey(dt => dt.Id);

           
           
        }
    }
}

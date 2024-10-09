using MedicalRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecords.Data.DBContext
{
    public class MedicalRecordsDBContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        public MedicalRecordsDBContext(DbContextOptions<MedicalRecordsDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
            .HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId);

            modelBuilder.Entity<Appointment>()
            .HasMany(a => a.Prescriptions)
            .WithOne(p => p.Appointment)
            .HasForeignKey(p => p.AppointmentId);
        }
    }
}

using MedicalRecords.Data.DBContext;
using MedicalRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public static class DataSeeder
{
    public static void Seed(MedicalRecordsDBContext context)
    {
        context.Database.EnsureCreated();

        if (context.Patients.Any() || context.Doctors.Any() || context.Appointments.Any() || context.Prescriptions.Any())
        {
            return; 
        }

        var patients = new List<Patient>
        {
            new Patient { Id = Guid.NewGuid(), Name = "Brizan Were", DateOfBirth = new DateTime(1980, 1, 1), Address = "123 Main St" },
            new Patient { Id = Guid.NewGuid(), Name = "June Helderly", DateOfBirth = new DateTime(1970, 5, 15), Address = "456 Oak St" }
        };

        var doctors = new List<Doctor>
        {
            new Doctor { Id = Guid.NewGuid(), Name = "Dr. Jerry Lokoroi", Address = "044 Lodwar Town", Specialization = "Neurology" },
            new Doctor { Id = Guid.NewGuid(), Name = "Dr. Bob Brown", Address = "50334 Loropio Village", Specialization = "Cardiology" }
        };

        var appointments = new List<Appointment>
        {
            new Appointment { Id = Guid.NewGuid(), Date = DateTime.Now.AddDays(1), PatientId = patients[0].Id, DoctorId = doctors[0].Id },
            new Appointment { Id = Guid.NewGuid(), Date = DateTime.Now.AddDays(2), PatientId = patients[1].Id, DoctorId = doctors[1].Id }
        };

        var prescriptions = new List<Prescription>
        {
            new Prescription { Id = Guid.NewGuid(), Medication = "Medication A", Dosage = "10mg", AppointmentId = appointments[0].Id },
            new Prescription { Id = Guid.NewGuid(), Medication = "Medication B", Dosage = "20mg", AppointmentId = appointments[1].Id }
        };

        context.Patients.AddRange(patients);
        context.Doctors.AddRange(doctors);
        context.Appointments.AddRange(appointments);
        context.Prescriptions.AddRange(prescriptions);

        context.SaveChanges();
    }
}

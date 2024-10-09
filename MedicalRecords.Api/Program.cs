using Microsoft.EntityFrameworkCore;
using MedicalRecords.Data.Repositories;
using MedicalRecords.Domain.Contracts;
using MedicalRecords.Data.DBContext;
using MedicalRecords.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MedicalRecordsDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MedicalRecordsDbContext")));

// Register repositories
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();

// Add services for Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure database is created and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MedicalRecordsDBContext>();
    DataSeeder.Seed(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Full CRUD and Specific Query Endpoints

// ========================== APPOINTMENTS ==========================
app.MapGet("/api/appointments", async (IAppointmentRepository repository) =>
{
    var appointments = await repository.GetAllAsync();
    return Results.Ok(appointments);
})
.WithName("GetAllAppointments")
.WithTags("Appointments")
.WithOpenApi();

app.MapGet("/api/appointments/{id:guid}", async (Guid id, IAppointmentRepository repository) =>
{
    var appointment = await repository.GetByIdAsync(id);
    return appointment is not null ? Results.Ok(appointment) : Results.NotFound();
})
.WithName("GetAppointmentById")
.WithTags("Appointments")
.WithOpenApi();

app.MapPost("/api/appointments", async (IAppointmentRepository repository, Appointment appointment) =>
{
    await repository.AddAsync(appointment);
    return Results.Created($"/api/appointments/{appointment.Id}", appointment);
})
.WithName("CreateAppointment")
.WithTags("Appointments")
.WithOpenApi();

app.MapPut("/api/appointments/{id:guid}", async (Guid id, IAppointmentRepository repository, Appointment updatedAppointment) =>
{
    var appointment = await repository.GetByIdAsync(id);
    if (appointment is null) return Results.NotFound();

    appointment.Date = updatedAppointment.Date;
    appointment.PatientId = updatedAppointment.PatientId;
    appointment.DoctorId = updatedAppointment.DoctorId;

    await repository.UpdateAsync(appointment);
    return Results.NoContent();
})
.WithName("UpdateAppointment")
.WithTags("Appointments")
.WithOpenApi();

app.MapDelete("/api/appointments/{id:guid}", async (Guid id, IAppointmentRepository repository) =>
{
    var appointment = await repository.GetByIdAsync(id);
    if (appointment is null) return Results.NotFound();

    await repository.DeleteAsync(id);
    return Results.NoContent();
})
.WithName("DeleteAppointment")
.WithTags("Appointments")
.WithOpenApi();

// ========================== DOCTORS ==========================
app.MapGet("/api/doctors", async (IDoctorRepository repository) =>
{
    var doctors = await repository.GetAllAsync();
    return Results.Ok(doctors);
})
.WithName("GetAllDoctors")
.WithTags("Doctors")
.WithOpenApi();

app.MapGet("/api/doctors/{id:guid}", async (Guid id, IDoctorRepository repository) =>
{
    var doctor = await repository.GetByIdAsync(id);
    return doctor is not null ? Results.Ok(doctor) : Results.NotFound();
})
.WithName("GetDoctorById")
.WithTags("Doctors")
.WithOpenApi();

app.MapPost("/api/doctors", async (IDoctorRepository repository, Doctor doctor) =>
{
    await repository.AddAsync(doctor);
    return Results.Created($"/api/doctors/{doctor.Id}", doctor);
})
.WithName("CreateDoctor")
.WithTags("Doctors")
.WithOpenApi();

app.MapPut("/api/doctors/{id:guid}", async (Guid id, IDoctorRepository repository, Doctor updatedDoctor) =>
{
    var doctor = await repository.GetByIdAsync(id);
    if (doctor is null) return Results.NotFound();

    doctor.Name = updatedDoctor.Name;
    doctor.Specialization = updatedDoctor.Specialization;

    await repository.UpdateAsync(doctor);
    return Results.NoContent();
})
.WithName("UpdateDoctor")
.WithTags("Doctors")
.WithOpenApi();

app.MapDelete("/api/doctors/{id:guid}", async (Guid id, IDoctorRepository repository) =>
{
    var doctor = await repository.GetByIdAsync(id);
    if (doctor is null) return Results.NotFound();

    await repository.DeleteAsync(id);
    return Results.NoContent();
})
.WithName("DeleteDoctor")
.WithTags("Doctors")
.WithOpenApi();

// ========================== PATIENTS ==========================
app.MapGet("/api/patients", async (IPatientRepository repository) =>
{
    var patients = await repository.GetAllAsync();
    return Results.Ok(patients);
})
.WithName("GetAllPatients")
.WithTags("Patients")
.WithOpenApi();

app.MapGet("/api/patients/{id:guid}", async (Guid id, IPatientRepository repository) =>
{
    var patient = await repository.GetByIdAsync(id);
    return patient is not null ? Results.Ok(patient) : Results.NotFound();
})
.WithName("GetPatientById")
.WithTags("Patients")
.WithOpenApi();

app.MapPost("/api/patients", async (IPatientRepository repository, Patient patient) =>
{
    await repository.AddAsync(patient);
    return Results.Created($"/api/patients/{patient.Id}", patient);
})
.WithName("CreatePatient")
.WithTags("Patients")
.WithOpenApi();

app.MapPut("/api/patients/{id:guid}", async (Guid id, IPatientRepository repository, Patient updatedPatient) =>
{
    var patient = await repository.GetByIdAsync(id);
    if (patient is null) return Results.NotFound();

    patient.Name = updatedPatient.Name;
    patient.DateOfBirth = updatedPatient.DateOfBirth;
    patient.Address = updatedPatient.Address;

    await repository.UpdateAsync(patient);
    return Results.NoContent();
})
.WithName("UpdatePatient")
.WithTags("Patients")
.WithOpenApi();

app.MapDelete("/api/patients/{id:guid}", async (Guid id, IPatientRepository repository) =>
{
    var patient = await repository.GetByIdAsync(id);
    if (patient is null) return Results.NotFound();

    await repository.DeleteAsync(id);
    return Results.NoContent();
})
.WithName("DeletePatient")
.WithTags("Patients")
.WithOpenApi();

// ========================== PRESCRIPTIONS ==========================
app.MapGet("/api/prescriptions", async (IPrescriptionRepository repository) =>
{
    var prescriptions = await repository.GetAllAsync();
    return Results.Ok(prescriptions);
})
.WithName("GetAllPrescriptions")
.WithTags("Prescriptions")
.WithOpenApi();

app.MapGet("/api/prescriptions/{id:guid}", async (Guid id, IPrescriptionRepository repository) =>
{
    var prescription = await repository.GetByIdAsync(id);
    return prescription is not null ? Results.Ok(prescription) : Results.NotFound();
})
.WithName("GetPrescriptionById")
.WithTags("Prescriptions")
.WithOpenApi();

app.MapPost("/api/prescriptions", async (IPrescriptionRepository repository, Prescription prescription) =>
{
    await repository.AddAsync(prescription);
    return Results.Created($"/api/prescriptions/{prescription.Id}", prescription);
})
.WithName("CreatePrescription")
.WithTags("Prescriptions")
.WithOpenApi();

app.MapPut("/api/prescriptions/{id:guid}", async (Guid id, IPrescriptionRepository repository, Prescription updatedPrescription) =>
{
    var prescription = await repository.GetByIdAsync(id);
    if (prescription is null) return Results.NotFound();

    prescription.Medication = updatedPrescription.Medication;
    prescription.Dosage = updatedPrescription.Dosage;
    prescription.AppointmentId = updatedPrescription.AppointmentId;

    await repository.UpdateAsync(prescription);
    return Results.NoContent();
})
.WithName("UpdatePrescription")
.WithTags("Prescriptions")
.WithOpenApi();

app.MapDelete("/api/prescriptions/{id:guid}", async (Guid id, IPrescriptionRepository repository) =>
{
    var prescription = await repository.GetByIdAsync(id);
    if (prescription is null) return Results.NotFound();

    await repository.DeleteAsync(id);
    return Results.NoContent();
})
.WithName("DeletePrescription")
.WithTags("Prescriptions")
.WithOpenApi();

// ========================== SPECIFIC QUERIES ==========================

// Get all Appointments for a specific doctor
app.MapGet("/api/doctors/{doctorId:guid}/appointments", async (Guid doctorId, IAppointmentRepository repository) =>
{
    var appointments = await repository.GetAppointmentsByDoctorIdAsync(doctorId);
    return appointments.Any() ? Results.Ok(appointments) : Results.NotFound();
})
.WithName("GetAppointmentsForDoctor")
.WithTags("Doctors", "Appointments")
.WithOpenApi();

// Get all prescriptions for a specific patient
app.MapGet("/api/patients/{patientId:guid}/prescriptions", async (Guid patientId, IPrescriptionRepository repository) =>
{
    var prescriptions = await repository.GetPrescriptionsByPatientIdAsync(patientId);
    return prescriptions.Any() ? Results.Ok(prescriptions) : Results.NotFound();
})
.WithName("GetPrescriptionsForPatient")
.WithTags("Patients", "Prescriptions")
.WithOpenApi();

app.Run();

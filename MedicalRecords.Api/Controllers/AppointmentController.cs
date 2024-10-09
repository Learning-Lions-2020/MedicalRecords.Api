using MedicalRecords.Api.DTOs;
using MedicalRecords.Data.DBContext;
using MedicalRecords.Domain.Contracts;
using MedicalRecords.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly MedicalRecordsDBContext _context;
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentController(MedicalRecordsDBContext context, IAppointmentRepository appointmentRepository)
    {
        _context = context;
        _appointmentRepository = appointmentRepository;
    }

    // GET: api/appointments
    [HttpGet]
    public async Task<IActionResult> GetAllAppointments()
    {
        var appointments = await _appointmentRepository.GetAllAsync();

        var appointmentDTOs = appointments.Select(a => new AppointmentDTO
        {
            Id = a.Id,
            Date = a.Date,
            PatientId = a.PatientId,
            PatientName = a.Patient?.Name ?? "Unknown Patient",
            DoctorId = a.DoctorId,
            DoctorName = a.Doctor?.Name ?? "Unknown Doctor",
            Prescriptions = a.Prescriptions?.Select(pr => new PrescriptionDTO 
            {
                Id = pr.Id,
                Medication = pr.Medication,
                Dosage = pr.Dosage
            }).ToList() 
        }).ToList();

        return Ok(appointmentDTOs);
    }

    // GET: api/appointments/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDTO>> GetAppointmentById(Guid id)
    {
        var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(id);
        if (appointment == null) return NotFound();

        var appointmentDTO = new AppointmentDTO
        {
            Id = appointment.Id,
            Date = appointment.Date,
            PatientId = appointment.PatientId,
            PatientName = appointment.Patient?.Name ?? "Unknown Patient",
            DoctorId = appointment.DoctorId,
            DoctorName = appointment.Doctor?.Name ?? "Unknown Doctor",
            Prescriptions = appointment.Prescriptions?.Select(pr => new PrescriptionDTO
            {
                Id = pr.Id,
                Medication = pr.Medication,
                Dosage = pr.Dosage
            }).ToList()
        };

        return Ok(appointmentDTO);
    }

    // POST: api/appointments
    [HttpPost]
    public async Task<IActionResult> AddAppointment([FromBody] AppointmentDTO appointmentDTO)
    {
        // Check if the Doctor exists
        var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == appointmentDTO.DoctorId);
        if (!doctorExists)
        {
            return BadRequest($"Doctor with Id {appointmentDTO.DoctorId} does not exist.");
        }

        // Check if the Patient exists
        var patientExists = await _context.Patients.AnyAsync(p => p.Id == appointmentDTO.PatientId);
        if (!patientExists)
        {
            return BadRequest($"Patient with Id {appointmentDTO.PatientId} does not exist.");
        }

        // Create the new appointment
        var appointment = new Appointment
        {
            Date = appointmentDTO.Date,
            PatientId = appointmentDTO.PatientId,
            DoctorId = appointmentDTO.DoctorId
        };

        await _appointmentRepository.AddAsync(appointment);

        // Return Created response with the appointment DTO
        return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointmentDTO);
    }

    // PUT: api/appointments/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] AppointmentDTO appointmentDTO)
    {
        if (id != appointmentDTO.Id) return BadRequest();

        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null) return NotFound();

        appointment.Date = appointmentDTO.Date;
        appointment.PatientId = appointmentDTO.PatientId;
        appointment.DoctorId = appointmentDTO.DoctorId;


        await _appointmentRepository.UpdateAsync(appointment);
        return NoContent();
    }

    // DELETE: api/appointments/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAppointment(Guid id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null) return NotFound();

        await _appointmentRepository.DeleteAsync(id);
        return NoContent();
    }

    // GET: api/appointments/patient/{patientId}
    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetAppointmentsForPatient(Guid patientId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsForPatientAsync(patientId);
        if (appointments == null || !appointments.Any()) return NotFound();

        var appointmentDTOs = appointments.Select(a => new AppointmentDTO
        {
            Id = a.Id,
            Date = a.Date,
            PatientId = a.PatientId,
            PatientName = a.Patient?.Name ?? "Unknown Patient",
            DoctorId = a.DoctorId,
            DoctorName = a.Doctor?.Name ?? "Unknown Doctor",
            Prescriptions = a.Prescriptions?.Select(pr => new PrescriptionDTO
            {
                Id = pr.Id,
                Medication = pr.Medication,
                Dosage = pr.Dosage
            }).ToList() 
        }).ToList();

        return Ok(appointmentDTOs);
    }

    // GET: api/appointments/doctor/{doctorId}
    [HttpGet("doctor/{doctorId:guid}")]
    public async Task<IActionResult> GetAppointmentsForDoctor(Guid doctorId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsForDoctorAsync(doctorId);
        if (appointments == null || !appointments.Any()) return NotFound();

        var appointmentDTOs = appointments.Select(a => new AppointmentDTO
        {
            Id = a.Id,
            Date = a.Date,
            PatientId = a.PatientId,
            PatientName = a.Patient?.Name ?? "Unknown Patient",
            DoctorId = a.DoctorId,
            DoctorName = a.Doctor?.Name ?? "Unknown Doctor",
            Prescriptions = a.Prescriptions?.Select(pr => new PrescriptionDTO
            {
                Id = pr.Id,
                Medication = pr.Medication,
                Dosage = pr.Dosage
            }).ToList() 
        }).ToList();

        return Ok(appointmentDTOs);
    }
}

using MedicalRecords.Data.DBContext;
using MedicalRecords.Domain.Contracts;
using MedicalRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    private readonly MedicalRecordsDBContext _context;

    public AppointmentRepository(MedicalRecordsDBContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _context.Appointments.ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
    {
        return await _context.Appointments.Where(a => a.DoctorId == doctorId).ToListAsync();
    }

    public async Task<Appointment> GetByIdAsync(Guid id)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Prescriptions)  // Ensure the prescription is loaded
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Appointment> GetAppointmentWithDetailsAsync(Guid id)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Prescriptions)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsForPatientAsync(Guid patientId)
    {
        return await _context.Appointments
            .Where(a => a.PatientId == patientId)
            .Include(a => a.Doctor)
            .Include(a => a.Prescriptions)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsForDoctorAsync(Guid doctorId)
    {
        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId)
            .Include(a => a.Patient)
            .Include(a => a.Prescriptions)
            .ToListAsync();
    }

    public async Task<Appointment> GetAppointmentWithPrescriptionAsync(Guid appointmentId)
    {
        return await _context.Appointments
            .Include(a => a.Prescriptions)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);
    }

    public async Task DeleteAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}

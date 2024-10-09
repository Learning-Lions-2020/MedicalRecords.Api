using MedicalRecords.Data.DBContext;
using MedicalRecords.Domain.Entities;
using MedicalRecords.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecords.Data.Repositories
{
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        private readonly MedicalRecordsDBContext _context;

        public PatientRepository(MedicalRecordsDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            return await _context.Prescriptions.ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(Guid patientId)
        {
            return await _context.Prescriptions.Where(p => p.Appointment.PatientId == patientId).ToListAsync();
        }
        public async Task<IEnumerable<Patient>> GetAllPatientsWithAppointmentsAsync()
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .ToListAsync();
        }

        public async Task<Patient> GetPatientWithAppointmentDetailsAsync(Guid patientId)
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(p => p.Id == patientId);
        }

        public async Task<IEnumerable<Patient>> GetRecentPatientsAsync(DateTime fromDate)
        {
            return await _context.Patients
                .Where(p => p.CreatedDate >= fromDate)
                .ToListAsync();
        }

        public async Task<Patient> GetByIdAsync(Guid id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await SaveChangesAsync(); 
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

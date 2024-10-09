using MedicalRecords.Data.DBContext;
using MedicalRecords.Domain.Contracts;
using MedicalRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecords.Data.Repositories
{
    public class PrescriptionRepository : BaseRepository<Prescription>, IPrescriptionRepository
    {
        private readonly MedicalRecordsDBContext _context;

        public PrescriptionRepository(MedicalRecordsDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Prescription> GetByIdAsync(Guid id)
        {
            return await _context.Prescriptions.FindAsync(id);
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(Guid patientId)
        {
            return await _context.Prescriptions
                .Where(p => p.Appointment.PatientId == patientId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Prescription>> GetPrescriptionsForPatientAsync(Guid patientId)
        {
            return await _context.Prescriptions
                .Include(p => p.Appointment)
                .Where(p => p.Appointment.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsForAppointmentAsync(Guid appointmentId)
        {
            return await _context.Prescriptions
                .Where(p => p.AppointmentId == appointmentId)
                .ToListAsync();
        }

        public override async Task DeleteAsync(Guid id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
                await SaveChangesAsync(); 
            }
        }
    }
}

using MedicalRecords.Data.DBContext;
using MedicalRecords.Domain.Contracts;
using MedicalRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecords.Data.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        private readonly MedicalRecordsDBContext _context;

        public DoctorRepository(MedicalRecordsDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsByPatientIdAsync(Guid patientId)
        {
            return await _context.Doctors.Where(d => d.Appointments.Any(a => a.PatientId == patientId)).ToListAsync();
        }
        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsWithAppointmentsAsync()
        {
            return await _context.Doctors
                .Include(d => d.Appointments)
                .ToListAsync();
        }

        public async Task<Doctor> GetDoctorByIdAsync(Guid doctorId)
        {
            return await _context.Doctors
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.Id == doctorId);
        }

        public async Task<Doctor> GetByIdAsync(Guid id)
        {
            return await _context.Doctors.FindAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var doctor = await GetByIdAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using MedicalRecords.Domain.Entities;

namespace MedicalRecords.Domain.Contracts
{
    public interface IDoctorRepository : IBaseRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<IEnumerable<Doctor>> GetDoctorsByPatientIdAsync(Guid patientId);

        Task<Doctor> GetDoctorByIdAsync(Guid doctorId);
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<IEnumerable<Doctor>> GetDoctorsWithAppointmentsAsync(); // Retrieves doctors with their appointments
    }
}

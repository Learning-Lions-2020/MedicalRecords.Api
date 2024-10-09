using MedicalRecords.Domain.Entities;

namespace MedicalRecords.Domain.Contracts
{
    public interface IPatientRepository : IBaseRepository<Patient>
    {
        Task<IEnumerable<Patient>> GetAllPatientsWithAppointmentsAsync(); // Retrieves all patients along with their appointment history.
        Task<Patient> GetPatientWithAppointmentDetailsAsync(Guid patientId); // Retrieves a specific patient along with their appointments in detail.
        Task<IEnumerable<Patient>> GetRecentPatientsAsync(DateTime fromDate); // Get patients from a specific date
    }
}

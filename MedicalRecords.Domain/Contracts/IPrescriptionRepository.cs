using MedicalRecords.Domain.Entities;

namespace MedicalRecords.Domain.Contracts
{
    public interface IPrescriptionRepository : IBaseRepository<Prescription>
    {
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(Guid patientId);

        Task<IEnumerable<Prescription>> GetPrescriptionsForPatientAsync(Guid patientId); // Retrieves all prescriptions for a given patient.
        Task<IEnumerable<Prescription>> GetPrescriptionsForAppointmentAsync(Guid appointmentId); // Retrieves prescriptions for a specific appointment.
    }
}

using MedicalRecords.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecords.Domain.Contracts
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId);

        Task<Appointment> GetAppointmentWithDetailsAsync(Guid appointmentId); // Retrieve an appointment with patient, doctor, and prescription details
        Task<IEnumerable<Appointment>> GetAppointmentsForPatientAsync(Guid patientId); // Retrieve all appointments for a specific patient
        Task<IEnumerable<Appointment>> GetAppointmentsForDoctorAsync(Guid doctorId); // Retrieve all appointments for a specific doctor
        Task<Appointment> GetAppointmentWithPrescriptionAsync(Guid appointmentId); // Retrieve an appointment along with its prescription
    }

}

using System;

namespace MedicalRecords.Domain.Entities
{
    public class Prescription
    {
        public Guid Id { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public Guid AppointmentId { get; set; }

        // Navigation property back to Appointment
        public virtual Appointment Appointment { get; set; }
    }
}

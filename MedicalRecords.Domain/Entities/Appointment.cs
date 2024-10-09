namespace MedicalRecords.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        // Prescriptions navigation property
        public ICollection<Prescription> Prescriptions { get; set; } 
    }
}

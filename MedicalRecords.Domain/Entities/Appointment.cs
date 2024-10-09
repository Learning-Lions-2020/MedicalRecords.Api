namespace MedicalRecords.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; } 
    }
}

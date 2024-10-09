namespace MedicalRecords.Api.DTOs
{
    public class PrescriptionDTO
    {
        public Guid Id { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public Guid AppointmentId { get; set; }
    }
}

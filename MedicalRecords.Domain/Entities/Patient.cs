namespace MedicalRecords.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}

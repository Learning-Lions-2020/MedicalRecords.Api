namespace MedicalRecords.Domain.Entities
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Specialization { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public Doctor() { }

        public Doctor(string name, DateTime dateOfBirth, string address)
        {
            Id = Guid.NewGuid();
            Name = name;
            DateOfBirth = dateOfBirth;
            Address = address;
        }
    }
}

﻿namespace MedicalRecords.Api.DTOs
{
    public class PatientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public ICollection<AppointmentDTO> Appointments { get; set; } 
    }
}

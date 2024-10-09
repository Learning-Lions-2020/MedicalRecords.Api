using System;
using System.Collections.Generic;

namespace MedicalRecords.Api.DTOs
{
    public class DoctorDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public ICollection<AppointmentDTO> Appointments { get; set; } = new List<AppointmentDTO>();
    }
}

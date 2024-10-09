using MedicalRecords.Api.DTOs; // Make sure you have the appropriate namespaces
using MedicalRecords.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecords.Api.DTOs
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public List<PrescriptionDTO> Prescriptions { get; set; } = new List<PrescriptionDTO>();
    }
}

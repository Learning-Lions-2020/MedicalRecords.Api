using MedicalRecords.Api.DTOs;
using MedicalRecords.Domain.Contracts;
using MedicalRecords.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MedicalRecords.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        // GET: api/patients
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllAsync();

            // Map to DTOs
            var patientDTOs = patients.Select(p => new PatientDTO
            {
                Id = p.Id,
                Name = p.Name,
                DateOfBirth = p.DateOfBirth,
                Address = p.Address,
                Appointments = p.Appointments?.Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    Date = a.Date,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor?.Name, 
                    Prescriptions = a.Prescriptions?.Select(pr => new PrescriptionDTO
                    {
                        Id = pr.Id,
                        Medication = pr.Medication,
                        Dosage = pr.Dosage
                    }).ToList() // Map prescriptions to DTO
                }).ToList()
            }).ToList();

            return Ok(patientDTOs);
        }

        // GET: api/patients/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(Guid id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) return NotFound();

            // Map to DTO
            var patientDTO = new PatientDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Address,
                Appointments = patient.Appointments?.Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    Date = a.Date,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor?.Name, 
                    Prescriptions = a.Prescriptions?.Select(pr => new PrescriptionDTO
                    {
                        Id = pr.Id,
                        Medication = pr.Medication,
                        Dosage = pr.Dosage
                    }).ToList() // Map prescriptions to DTO
                }).ToList()
            };

            return Ok(patientDTO);
        }

        // POST: api/patients
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] PatientDTO patientDTO)
        {
            if (patientDTO == null) return BadRequest("Patient data is null.");

            // Map from DTO to entity
            var patient = new Patient
            {
                Name = patientDTO.Name,
                DateOfBirth = patientDTO.DateOfBirth,
                Address = patientDTO.Address
            };

            await _patientRepository.AddAsync(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patientDTO);
        }

        // PUT: api/patients/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] PatientDTO patientDTO)
        {
            if (id != patientDTO.Id) return BadRequest();

            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) return NotFound();

            patient.Name = patientDTO.Name;
            patient.DateOfBirth = patientDTO.DateOfBirth;
            patient.Address = patientDTO.Address;

            await _patientRepository.UpdateAsync(patient);
            return NoContent();
        }

        // DELETE: api/patients/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) return NotFound();

            await _patientRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

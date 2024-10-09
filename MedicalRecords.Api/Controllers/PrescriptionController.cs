using MedicalRecords.Api.DTOs;
using MedicalRecords.Domain.Contracts;
using MedicalRecords.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecords.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionRepository _prescriptionRepository; // Remove <Prescription> from interface type

        public PrescriptionController(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        // GET: api/prescriptions
        [HttpGet]
        public async Task<IActionResult> GetAllPrescriptions()
        {
            var prescriptions = await _prescriptionRepository.GetAllAsync();

            if (prescriptions == null || !prescriptions.Any())
                return NotFound("No prescriptions found.");

            var prescriptionDTOs = prescriptions.Select(p => new PrescriptionDTO
            {
                Id = p.Id,
                Medication = p.Medication,
                Dosage = p.Dosage,
                AppointmentId = p.AppointmentId
            }).ToList();

            return Ok(prescriptionDTOs);
        }

        // GET: api/prescriptions/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPrescription(Guid id)
        {
            var prescription = await _prescriptionRepository.GetByIdAsync(id);
            if (prescription == null) return NotFound();

            var prescriptionDTO = new PrescriptionDTO
            {
                Id = prescription.Id,
                Medication = prescription.Medication,
                Dosage = prescription.Dosage,
                AppointmentId = prescription.AppointmentId
            };

            return Ok(prescriptionDTO);
        }

        // POST: api/prescriptions
        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionDTO prescriptionDTO)
        {
            if (prescriptionDTO == null) return BadRequest("Prescription data is null.");

            var prescription = new Prescription
            {
                Id = Guid.NewGuid(), // Ensure a new Guid is generated for the new prescription
                Medication = prescriptionDTO.Medication,
                Dosage = prescriptionDTO.Dosage,
                AppointmentId = prescriptionDTO.AppointmentId
            };

            await _prescriptionRepository.AddAsync(prescription);
            await _prescriptionRepository.SaveChangesAsync(); // Ensure the changes are saved

            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.Id }, prescriptionDTO);
        }

        // PUT: api/prescriptions/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePrescription(Guid id, [FromBody] PrescriptionDTO prescriptionDTO)
        {
            if (id != prescriptionDTO.Id) return BadRequest("ID mismatch.");

            var prescription = await _prescriptionRepository.GetByIdAsync(id);
            if (prescription == null) return NotFound();

            prescription.Medication = prescriptionDTO.Medication;
            prescription.Dosage = prescriptionDTO.Dosage;

            await _prescriptionRepository.UpdateAsync(prescription);
            await _prescriptionRepository.SaveChangesAsync(); // Ensure the changes are saved

            return NoContent();
        }

        // DELETE: api/prescriptions/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePrescription(Guid id)
        {
            var prescription = await _prescriptionRepository.GetByIdAsync(id);
            if (prescription == null) return NotFound();

            await _prescriptionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

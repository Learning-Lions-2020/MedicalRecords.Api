using MedicalRecords.Api.DTOs;
using MedicalRecords.Domain.Contracts;
using MedicalRecords.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MedicalRecords.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorRepository.GetAllDoctorsAsync();
            var doctorDTOs = doctors.Select(d => new DoctorDTO
            {
                Id = d.Id,
                Name = d.Name,
                DateOfBirth = d.DateOfBirth,
                Address = d.Address
            }).ToList();

            return Ok(doctorDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(Guid id)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound("Doctor not found");

            var doctorDTO = new DoctorDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                DateOfBirth = doctor.DateOfBirth,
                Address = doctor.Address
            };

            return Ok(doctorDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorDTO doctorDTO)
        {
            if (doctorDTO == null) return BadRequest("Doctor data is invalid");

            var doctor = new Doctor(doctorDTO.Name, doctorDTO.DateOfBirth, doctorDTO.Address);

            await _doctorRepository.AddAsync(doctor);

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(Guid id, [FromBody] DoctorDTO doctorDTO)
        {
            if (id != doctorDTO.Id) return BadRequest("Doctor ID mismatch");

            var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound("Doctor not found");

            doctor.Name = doctorDTO.Name;
            doctor.DateOfBirth = doctorDTO.DateOfBirth;
            doctor.Address = doctorDTO.Address;

            await _doctorRepository.UpdateAsync(doctor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound("Doctor not found");

            await _doctorRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

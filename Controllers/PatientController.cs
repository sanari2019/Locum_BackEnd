using Microsoft.AspNetCore.Mvc;
using Locum_Backend.Repositories;
using Locum_Backend.Models;
using System;

namespace Locum_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientRepository _patientRepository;
        private readonly ApprovalRequestRepository _approvalRequest;
        private readonly UserRepository _userRequest;
        private readonly ValidatedPatientRepository _validpatientRequest;
        private readonly WardRepository _wardRepository;

        public PatientController(WardRepository wardRepository, PatientRepository patientRepository, ApprovalRequestRepository approvalRequest, UserRepository userRequest, ValidatedPatientRepository validpatientRequest)
        {
            _patientRepository = patientRepository;
            _approvalRequest = approvalRequest;
            _userRequest = userRequest;
            _validpatientRequest = validpatientRequest;
            _wardRepository = wardRepository;
        }

        [HttpGet]
        public IActionResult GetPatients()
        {
            var patients = _patientRepository.GetPatients();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public IActionResult GetPatient(int id)
        {
            var patient = _patientRepository.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public IActionResult CreatePatient(RequestData request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            var validd = _validpatientRequest.GetValidatedPatient(request.patient);
            var ward = _wardRepository.GetDepartmentById(request.approvalRequest.ward_id);
            request.approvalRequest.department_id = request.approvalRequest.ward_id;

            if (validd.Status != "0")
            {
                if (validd.WardName == ward.ward_name)
                {
                    var pat = new Patient
                    {
                        first_Name = validd.Patient,
                        last_Name = validd.Patient,
                        uhid = request.patient.UHID,
                        wardName = validd.WardName,
                        roomNo = validd.RoomNo,
                        Is_Validated = true,
                        Created_At = DateTime.Now
                    };

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    // request.patient.Is_Validated = true;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    // request.patient.Created_At = DateTime.Now;
                    _patientRepository.InsertPatient(pat);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    request.approvalRequest.Created_At = DateTime.Now;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    request.approvalRequest.Date_Entered = DateTime.Now;
                    request.approvalRequest.Patient_Id = pat.Id;
                    _approvalRequest.InsertRequest(request.approvalRequest);
                    return Ok(validd);
                }
                validd.Comment = "Patient not in that ward";
            }


            return Ok(validd);
        }

        [HttpPost("updatepatient")]
        public IActionResult UpdatePatient([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest();
            }

            _patientRepository.UpdatePatient(patient);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            // var requestData = _patientRepository.GetValidatedPatientByIDandUserID(id);
            var patient = _patientRepository.GetPatientById(id);
            var request = _approvalRequest.GetRequestByPatientId(id);
            if (patient == null)
            {
                return NotFound();
            }

            _patientRepository.DeletePatient(patient);
            _approvalRequest.DeleteRequest(request);
            return NoContent();
        }

        [HttpGet("validatedpatients/{enteredByUserId}")]
        public IActionResult GetValidatedPatientsByUser(int enteredByUserId)
        {
            try
            {
                var validatedPatients = _patientRepository.GetValidatedPatientByUser(enteredByUserId);
                return Ok(validatedPatients);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // [HttpGet("validatedpatientsbyidanduserid/{requestId}")]
        // public IActionResult GetValidatedPatientsByIDAndUserID(int requestId)
        // {
        //     try
        //     {
        //         var validatedPatients = _patientRepository.GetValidatedPatientByIDandUserID(requestId);
        //         return Ok(validatedPatients);
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log or handle the exception appropriately
        //         return StatusCode(500, $"Internal Server Error: {ex.Message}");
        //     }
        // }

    }
}

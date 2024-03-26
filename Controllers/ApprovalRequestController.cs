using Microsoft.AspNetCore.Mvc;
using Locum_Backend;
using Locum_Backend.Repositories;
using Locum_Backend.Models;
using Locum_Backend.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Text;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;

namespace Locum_Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalRequestController : ControllerBase
    {
        private readonly ApprovalRequestRepository _requestMainRepository;
        private readonly RequestFormPatientRepository _requestFPRepository;
        private readonly PatientRepository _patientRepository;
        private readonly UserRepository _userRepository;
        private readonly UsersRolesRepository _userRolesRepository;
        private readonly EmployeeTypeRepository _emptypRepository;
        private readonly ApprovalRepository _approvalRepository;
        private readonly WardSupervisorRepository _wardsupervisorRepository;

        public ApprovalRequestController(WardSupervisorRepository wardsupervisorRepository, ApprovalRequestRepository requestMainRepository, RequestFormPatientRepository requestFPRepository, PatientRepository patientRepository, UserRepository userRepository, UsersRolesRepository userRolesRepository, EmployeeTypeRepository emptypRepository, ApprovalRepository approvalRepository)
        {
            _requestMainRepository = requestMainRepository;
            _requestFPRepository = requestFPRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _userRolesRepository = userRolesRepository;
            _emptypRepository = emptypRepository;
            _approvalRepository = approvalRepository;
            _wardsupervisorRepository = wardsupervisorRepository;
        }

        [HttpGet]
        public IActionResult GetRequests()
        {
            var requests = _requestMainRepository.GetRequests();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public IActionResult GetRequest(int id)
        {
            var request = _requestMainRepository.GetRequestById(id);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpPost]
        public IActionResult CreateRequest([FromBody] ApprovalRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            request.Date_Entered = DateTime.Now;
            _requestMainRepository.InsertRequest(request);
            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
        }

        [HttpPost("updaterequest")]
        public IActionResult UpdateRequest([FromBody] ApprovalRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }




            _requestMainRepository.UpdateRequest(request);
            return NoContent();
        }
        [HttpPost("updatereqninsert/{userid}")]
        public IActionResult UpdateRequestandInsertRequestData([FromBody] RequestFormPatient requestFormPatient)
        {
            if (requestFormPatient == null)
            {
                return BadRequest();
            }
            requestFormPatient.Is_Submitted = true;
            _requestFPRepository.InsertRequestFormPatient(requestFormPatient);
            var approvalRequest = _requestMainRepository.GetRequestsByUserIdFromView(requestFormPatient.userId);
            foreach (var requestItem in approvalRequest)
            {
                var req = _requestMainRepository.GetRequestById(requestItem.Id);
                req.Date_Entered = DateTime.Now;
                req.Created_At = DateTime.Now;
                req.request_Id = requestFormPatient.Approval_Request_Id;
                _requestMainRepository.UpdateRequest(req);
            }
            return Ok(approvalRequest);
        }
        [HttpPost("updaterequest/{userid}")]
        public IActionResult UpdateApprovalandRequest([FromBody] Approval approvalEntity)
        {
            if (approvalEntity == null)
            {
                return BadRequest();
            }

            approvalEntity.Approved_By_User_Id = approvalEntity.userId;
            _approvalRepository.UpdateApproval(approvalEntity);
            var approvalRequest = _requestFPRepository.GetRequestFormPatientById(approvalEntity.Approval_Request_Id);
            var info = _userRolesRepository.GetEmployeeTypeFromUserId(approvalEntity.Approved_By_User_Id);
            var employeeType = _emptypRepository.GetEmpTypeById(info.Employee_Type_Id);

            if (employeeType.type_Name == "CNO")
            {
                approvalRequest.Approval_Level_Id = employeeType.Approval_Level;
                _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
                return Ok("Approval updated successfully");

            }
            else if (employeeType.type_Name == "Supervisor")
            {
                approvalRequest.Approval_Level_Id = employeeType.Approval_Level;
                _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
                return Ok("Approval updated successfully");

            }


            return BadRequest("Invalid employee type");

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRequest(int id)
        {
            var request = _requestMainRepository.GetRequestById(id);
            if (request == null)
            {
                return NotFound();
            }

            _requestMainRepository.DeleteRequest(request);
            return NoContent();
        }
        [HttpGet("getapprovedrequests")]
        public ActionResult<IEnumerable<ApprovalRequest>> GetApprovedRequests()
        {
            try
            {
                var approvedRequests = _requestMainRepository.GetApprovedRequests();
                return Ok(approvedRequests);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getpendingrequest/{id}")]
        public ActionResult<User> GetPendingRequestsByUserId(int id)
        {
            return new OkObjectResult(_requestMainRepository.GetPendingRequestsByUserId(id));

        }
        [HttpGet("getdecidedrequest/{id}")]
        public ActionResult<User> GetDecidedRequestsByUserId(int id)
        {
            return new OkObjectResult(_requestMainRepository.GetDecidedRequestsByUserId(id));

        }
        [HttpGet("getsupapprovalrequest")]
        public ActionResult<User> GetSupApprovalRequests(int user_id)
        {
            var info = _userRolesRepository.GetEmployeeTypeFromUserId(user_id);
            var employeeType = _emptypRepository.GetEmpTypeById(info.Employee_Type_Id);
            var wardsupervisor = _wardsupervisorRepository.GetSupervisorByUserId(user_id);
            var wards = _wardsupervisorRepository.GetAllWardsBySupervisor(user_id);

            if (employeeType != null)
            {

            }
            foreach (var ward in wards)
            {

            }
            return NoContent();

        }
        [HttpGet("getcnopendingrequest/{user_id}")]
        public ActionResult<User> GetCnoPendingRequests(int user_id)
        {
            var info = _userRolesRepository.GetEmployeeTypeFromUserId(user_id);
            var employeeType = _emptypRepository.GetEmpTypeById(info.Employee_Type_Id);
            var wardsupervisor = _wardsupervisorRepository.GetSupervisorByUserId(user_id);
            var wards = _wardsupervisorRepository.GetAllWardsBySupervisor(user_id);

            if (employeeType.type_Name == "CNO")
            {
                return new OkObjectResult(_requestMainRepository.GetCnoPendingRequests());
            }
            else if (employeeType.type_Name == "Supervisor")
            {
                var requests = new List<RequestFormPatient>();
                foreach (var ward in wards)
                {
                    var wardRequests = _requestMainRepository.GetSupApprovalRequests(ward.ward_id);
                    requests.AddRange(wardRequests);
                }
                return new OkObjectResult(requests);
            }

            // Handle other cases or return an appropriate response if needed
            return BadRequest("Invalid employee type");
        }
        [HttpGet("req/{requestId}")]
        public ActionResult<ApprovalRequest> GetRequestByRequestId(int requestId)
        {
            var request = _requestMainRepository.GetRequestByRequestId(requestId);

            if (request == null)
            {
                return NotFound();
            }

            return Ok(request);
        }

        [HttpGet("allreq/{requestId}")]
        public ActionResult<List<ApprovalRequest>> GetRequestsByRequestId(int requestId)
        {
            var requests = _requestMainRepository.GetRequestsByRequestId(requestId);

            if (requests == null || requests.Count == 0)
            {
                return NotFound();
            }

            return Ok(requests);
        }
        [HttpGet("getApprovalDetails/{approvalRequestId}")]
        public ActionResult<List<ApprovalDetails>> GetApprovalDetails(int approval_Request_Id)
        {
            try
            {
                var approval_RequestId = _requestMainRepository.GetApprovalDetailssByRequestId(approval_Request_Id);

                if (approval_RequestId == null)
                {
                    return NotFound(); // Return 404 if no approval details found
                }

                return Ok(approval_RequestId);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }



}
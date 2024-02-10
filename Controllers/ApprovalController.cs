using Microsoft.AspNetCore.Mvc;
using Locum_Backend.Repositories;
using Locum_Backend.Models;

namespace Locum_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly ApprovalRepository _approvalRepository;
        private readonly RequestFormPatientRepository _requestFPRepository;
        private readonly ApprovalRequestRepository _requestMainRepository;
        private readonly PatientRepository _patientRepository;
        private readonly UserRepository _userRepository;
        private readonly UsersRolesRepository _userRolesRepository;
        private readonly EmployeeTypeRepository _emptypRepository;

        public ApprovalController(ApprovalRepository approvalRepository, RequestFormPatientRepository requestFPRepository, ApprovalRequestRepository requestMainRepository, PatientRepository patientRepository, UserRepository userRepository, UsersRolesRepository userRolesRepository, EmployeeTypeRepository emptypRepository)
        {
            _approvalRepository = approvalRepository;
            _requestFPRepository = requestFPRepository;
            _requestMainRepository = requestMainRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _userRolesRepository = userRolesRepository;
            _emptypRepository = emptypRepository;
        }

        [HttpGet]
        public IActionResult GetApprovals()
        {
            var approvals = _approvalRepository.GetApprovals();
            return Ok(approvals);
        }

        [HttpGet("{id}")]
        public IActionResult GetApproval(int id)
        {
            var approval = _approvalRepository.GetApprovalById(id);
            if (approval == null)
            {
                return NotFound();
            }
            return Ok(approval);
        }

        [HttpPost]
        public IActionResult CreateApproval([FromBody] Approval approval)
        {
            if (approval == null)
            {
                return BadRequest();
            }
            _approvalRepository.InsertApproval(approval);
            return CreatedAtAction(nameof(GetApproval), new { id = approval.Id }, approval);
        }

        [HttpPost("update")]
        public IActionResult UpdateApproval([FromBody] Approval approvalEntity)
        {
            try
            {
                _approvalRepository.UpdateApproval(approvalEntity);
                return Ok("Approval updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating approval: {ex.Message}");
            }
        }

        [HttpPost("updaterequest")]
        public IActionResult UpdateApprovalandRequest([FromBody] Approval approvalEntity)
        {
            if (approvalEntity == null)
            {
                return BadRequest();
            }
            // approvalEntity.Approval_Request_Id = approvalEntity.Id;
            int t1;
            if (approvalEntity.Approved_By_User_Id == 0)
            {
                t1 = 1;
            }
            else
            {
                t1 = 0;
            }
            var approvalRequest = _requestFPRepository.GetRequestFormPatientById(approvalEntity.Approval_Request_Id);
            approvalEntity.Approved_By_User_Id = approvalEntity.userId;
            var info = _userRolesRepository.GetEmployeeTypeFromUserId(approvalEntity.Approved_By_User_Id);
            var employeeType = _emptypRepository.GetEmpTypeById(info.Employee_Type_Id);
            //approvalEntity.Approval_Request_Id = approvalEntity.Id;

            if (employeeType.type_Name == "CNO")
            {

                approvalEntity.Status = "Approved";
                if (t1 == 1)
                {
                    _approvalRepository.UpdateApproval(approvalEntity);
                }
                else
                {
                    _approvalRepository.InsertApproval(approvalEntity);
                }

                approvalRequest.Approval_Level_Id = employeeType.Approval_Level;
                _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
                var approvalRequests = _requestMainRepository.GetRequestsByRequestId(approvalEntity.Approval_Request_Id);
                foreach (var req in approvalRequests)
                {
                    req.Status = approvalEntity.Status;
                    _requestMainRepository.UpdateRequest(req);
                }
                return Ok(approvalEntity);

            }
            else if (employeeType.type_Name == "Supervisor")
            {

                approvalEntity.Status = "Awaiting CNO approval";
                _approvalRepository.UpdateApproval(approvalEntity);
                approvalRequest.Approval_Level_Id = employeeType.Approval_Level;
                _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
                var approvalRequests = _requestMainRepository.GetRequestsByRequestId(approvalEntity.Approval_Request_Id);
                foreach (var req in approvalRequests)
                {
                    req.Status = approvalEntity.Status;
                    _requestMainRepository.UpdateRequest(req);
                }
                return Ok(approvalEntity);

            }


            return BadRequest("Invalid employee type");

        }

        // [HttpGet("approval-details/{approvalRequestId}")]
        // public IActionResult GetApprovalDetails(int approvalRequestId)
        // {
        //     try
        //     {
        //         var approvalDetails = _requestMainRepository.GetApprovalDetailsByRequestId(approvalRequestId);

        //         if (approvalDetails != null)
        //         {
        //             return Ok(approvalDetails);
        //         }
        //         else
        //         {
        //             return NotFound(); // Assuming NotFound is appropriate for a non-existent approval request
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log or handle the exception appropriately
        //         return StatusCode(500, $"Internal Server Error: {ex.Message}");
        //     }
        // }
        [HttpPost("updatedecline")]
        public IActionResult UpdateDeclineandRequest([FromBody] Approval approvalEntity)
        {
            if (approvalEntity == null)
            {
                return BadRequest();
            }
            // approvalEntity.Approval_Request_Id = approvalEntity.Id;
            int t1;
            if (approvalEntity.Approved_By_User_Id == 0)
            {
                t1 = 1;
            }
            else
            {
                t1 = 0;
            }
            var approvalRequest = _requestFPRepository.GetRequestFormPatientById(approvalEntity.Approval_Request_Id);
            approvalEntity.Approved_By_User_Id = approvalEntity.userId;
            var info = _userRolesRepository.GetEmployeeTypeFromUserId(approvalEntity.Approved_By_User_Id);
            var employeeType = _emptypRepository.GetEmpTypeById(info.Employee_Type_Id);
            //approvalEntity.Approval_Request_Id = approvalEntity.Id;
            approvalEntity.Status = "Decline by" + employeeType.type_Name;

            if (employeeType.type_Name == "CNO")
            {


                if (t1 == 1)
                {
                    _approvalRepository.UpdateApproval(approvalEntity);
                }
                else
                {
                    _approvalRepository.InsertApproval(approvalEntity);
                }

                approvalRequest.Approval_Level_Id = 0;
                _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
                var approvalRequests = _requestMainRepository.GetRequestsByRequestId(approvalEntity.Approval_Request_Id);
                foreach (var req in approvalRequests)
                {
                    req.Status = approvalEntity.Status;
                    _requestMainRepository.UpdateRequest(req);
                }
                return Ok(approvalEntity);

            }
            else if (employeeType.type_Name == "Supervisor")
            {
                _approvalRepository.UpdateApproval(approvalEntity);
                approvalRequest.Approval_Level_Id = 0;
                _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
                var approvalRequests = _requestMainRepository.GetRequestsByRequestId(approvalEntity.Approval_Request_Id);
                foreach (var req in approvalRequests)
                {
                    req.Status = approvalEntity.Status;
                    _requestMainRepository.UpdateRequest(req);
                }
                return Ok(approvalEntity);

            }


            return BadRequest("Invalid employee type");

        }



        [HttpGet("approval-details/{approvalRequestId}")]
        public IActionResult GetApprovalDetailsByUserId(int approvalRequestId)
        {
            try
            {
                var approvalDetails = _approvalRepository.GetApprovalDetailsByUserId(approvalRequestId);

                if (approvalDetails != null)
                {
                    return Ok(approvalDetails);
                }
                else
                {
                    return NotFound(); // Assuming NotFound is appropriate for a non-existent approval request
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        // Add other actions as needed
    }
}

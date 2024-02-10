using Microsoft.AspNetCore.Mvc;
using Locum_Backend.Models; // Make sure to include the necessary models
using Locum_Backend.Repositories; // Make sure to include the necessary repositories

[Route("api/[controller]")]
[ApiController]
public class RequestFormPatientController : ControllerBase
{
    private readonly RequestFormPatientRepository _requestFormPatientRepository;
    private readonly ApprovalRequestRepository _requestMainRepository;
    private readonly UsersRolesRepository _userRepository;

    private readonly EmployeeTypeRepository _employeeTypeRepository;
    private readonly ApprovalRepository _approvalRepository;


    public RequestFormPatientController(EmployeeTypeRepository employeeTypeRepository, UsersRolesRepository userRepository, RequestFormPatientRepository requestFormPatientRepository, ApprovalRequestRepository requestMainRepository, ApprovalRepository approvalRepository)
    {
        _requestFormPatientRepository = requestFormPatientRepository;
        _requestMainRepository = requestMainRepository;
        _userRepository = userRepository;
        _employeeTypeRepository = employeeTypeRepository;
        _approvalRepository = approvalRepository;
    }

    [HttpPost]
    public IActionResult InsertRequestFormPatient(RequestFormPatient requestFormPatient)
    {
        if (requestFormPatient == null)
        {
            return BadRequest();
        }
        else
        {
            var info = _userRepository.GetEmployeeTypeFromUserId(requestFormPatient.userId);
            var employeeType = _employeeTypeRepository.GetEmpTypeById(info.Employee_Type_Id);

            requestFormPatient.Is_Submitted = true;
            // Check the latest Approval_Request_Id
            var latestApprovalRequestId = _requestFormPatientRepository.GetLatestApprovalRequestId();

            // Set the Approval_Request_Id
            requestFormPatient.Approval_Request_Id = latestApprovalRequestId == null || latestApprovalRequestId == 0
            ? 1
            : latestApprovalRequestId + 1;


            requestFormPatient.Approval_Level_Id = employeeType.Approval_Level;


            _requestFormPatientRepository.InsertRequestFormPatient(requestFormPatient);
            var req = _requestMainRepository.GetRequestsByUserIdFromView(requestFormPatient.userId);
            foreach (var requestItem in req)
            {
                var reqq = _requestMainRepository.GetRequestById(requestItem.Id);
                reqq.Date_Entered = DateTime.Now;
                reqq.Created_At = DateTime.Now;
                reqq.request_Id = _requestFormPatientRepository.GetLatestApprovalRequestId();
                _requestMainRepository.UpdateRequest(reqq);
            }
            var approval = new Approval();
            approval.Status = "Pending";
            approval.Approval_Request_Id = _requestFormPatientRepository.GetLatestApprovalRequestId();
            _approvalRepository.InsertApproval(approval);

            return NoContent();
        }
    }




    [HttpPut("update")]
    public IActionResult UpdateRequestFormPatient(RequestFormPatient requestFormPatient)
    {
        if (requestFormPatient == null)
        {
            return BadRequest();
        }

        _requestFormPatientRepository.UpdateRequestFormPatient(requestFormPatient);

        return NoContent();
    }

    [HttpDelete("delete")]
    public IActionResult DeleteRequestFormPatient(RequestFormPatient requestFormPatient)
    {
        if (requestFormPatient == null)
        {
            return BadRequest();
        }

        _requestFormPatientRepository.DeleteRequestFormPatient(requestFormPatient);

        return NoContent();
    }

    [HttpGet("getall")]
    public IActionResult GetAllRequestFormPatients()
    {
        var requestFormPatients = _requestFormPatientRepository.GetRequestFormPatients();

        return Ok(requestFormPatients);
    }

    [HttpGet("{id}")]
    public IActionResult GetRequestFormPatientById(int id)
    {
        var requestFormPatient = _requestFormPatientRepository.GetRequestFormPatientById(id);

        if (requestFormPatient == null)
        {
            return NotFound();
        }

        return Ok(requestFormPatient);
    }
}

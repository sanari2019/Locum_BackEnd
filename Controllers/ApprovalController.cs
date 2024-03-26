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
        private EmailConfiguration _emailConfig;

        private readonly WardSupervisorRepository _wardsupervisorRepository;


        public ApprovalController(WardSupervisorRepository wardsupervisorRepository, ApprovalRepository approvalRepository, RequestFormPatientRepository requestFPRepository, ApprovalRequestRepository requestMainRepository, PatientRepository patientRepository, UserRepository userRepository, UsersRolesRepository userRolesRepository, EmployeeTypeRepository emptypRepository, EmailConfiguration emailConfig)
        {
            _approvalRepository = approvalRepository;
            _requestFPRepository = requestFPRepository;
            _requestMainRepository = requestMainRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _userRolesRepository = userRolesRepository;
            _emptypRepository = emptypRepository;
            this._emailConfig = emailConfig;
            _wardsupervisorRepository = wardsupervisorRepository;
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

        // [HttpPost("updaterequest")]
        // public IActionResult UpdateApprovalandRequest([FromBody] Approval approvalEntity)
        // {
        //     if (approvalEntity == null)
        //     {
        //         return BadRequest();
        //     }
        //     // approvalEntity.Approval_Request_Id = approvalEntity.Id;
        //     int t1;
        //     if (approvalEntity.Approved_By_User_Id == 0)
        //     {
        //         t1 = 1;
        //     }
        //     else
        //     {
        //         t1 = 0;
        //     }
        //     var approvalRequest = _requestFPRepository.GetRequestFormPatientById(approvalEntity.Approval_Request_Id);
        //     var request = _requestMainRepository.GetApprovalDetailsByRequestId(approvalEntity.Approval_Request_Id);
        //     var initiator = _userRepository.GetUser(request.entered_By_User_Id);
        //     approvalEntity.Approved_By_User_Id = approvalEntity.userId;
        //     var info = _userRolesRepository.GetEmployeeTypeFromUserId(approvalEntity.Approved_By_User_Id);
        //     var employeeType = _emptypRepository.GetEmpTypeById(info.Employee_Type_Id);
        //     // var wardsupo = _wardsupervisorRepository.GetSupervisorByWardId(approvalEntity.ApprovalRequest.ward_id);
        //     // var supo = _userRepository.GetUser(wardsupo.user_id);
        //     var info_user = _userRepository.GetUser(info.User_Id);
        //     //approvalEntity.Approval_Request_Id = approvalEntity.Id;

        //     if (employeeType.type_Name == "CNO")
        //     {

        //         approvalEntity.Status = "Approved";
        //         if (t1 == 1)
        //         {
        //             _approvalRepository.UpdateApproval(approvalEntity);
        //         }
        //         else
        //         {
        //             _approvalRepository.InsertApproval(approvalEntity);
        //         }

        //         approvalRequest.Approval_Level_Id = employeeType.Approval_Level;
        //         _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
        //         var approvalRequests = _requestMainRepository.GetRequestsByRequestId(approvalEntity.Approval_Request_Id);
        //         foreach (var req in approvalRequests)
        //         {
        //             req.Status = approvalEntity.Status;
        //             _requestMainRepository.UpdateRequest(req);
        //         }
        //         EmailSender _emailSender = new EmailSender(this._emailConfig);
        //         Email em = new Email();
        //         string logourl = "";//"https://evercaregroup.com/wp-content/uploads/2020/12/EVERCARE_LOGO_03_LEKKI_PRI_FC_RGB.png";
        //         string applink = "https://cafeteria.evercare.ng";
        //         string salutation = "Hello" + ",";
        //         string emailcontent = "An approval final respone have been initiated by " + info_user.First_Name + ' ' + info_user.Last_Name + " To" + " " + initiator.First_Name + " " + initiator.Last_Name + " Thanks for visiting Evercare's";
        //         string narration1 = " ";
        //         string econtent = em.HtmlMail("CNO Approval Response", applink, salutation, emailcontent, narration1, logourl);
        //         var message = new Message(new string[] { info_user.Email }, new string[] { initiator.Email }, "Locum Final Response Application", econtent);
        //         _emailSender.SendEmail(message);
        //         // return Ok(requestData);
        //         return Ok(approvalEntity);

        //     }
        //     else if (employeeType.type_Name == "Supervisor")
        //     {

        //         approvalEntity.Status = "Awaiting CNO approval";
        //         _approvalRepository.UpdateApproval(approvalEntity);
        //         approvalRequest.Approval_Level_Id = employeeType.Approval_Level;
        //         _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
        //         var approvalRequests = _requestMainRepository.GetRequestsByRequestId(approvalEntity.Approval_Request_Id);
        //         var cno = _userRolesRepository.GetUserRoleByEmpId(3);
        //         var cno_user = _userRepository.GetUser(cno.User_Id);
        //         foreach (var req in approvalRequests)
        //         {
        //             req.Status = approvalEntity.Status;
        //             _requestMainRepository.UpdateRequest(req);
        //         }

        //         EmailSender _emailSender = new EmailSender(this._emailConfig);
        //         Email em = new Email();
        //         string logourl = "";//"https://evercaregroup.com/wp-content/uploads/2020/12/EVERCARE_LOGO_03_LEKKI_PRI_FC_RGB.png";
        //         string applink = "https://cafeteria.evercare.ng";
        //         string salutation = "Hello" + " " + cno_user.First_Name + ",";
        //         string emailcontent =  info_user.First_Name + ' ' + info_user.Last_Name + "approved"  + initiator.First_Name + " " + initiator.Last_Name +  "request. visit link below to review";
        //         string narration1 = " ";
        //         string econtent = em.HtmlMail("Supervisor Locum Payment Approval", applink, salutation, emailcontent, narration1, logourl);
        //         var message = new Message(new string[] { info_user.Email }, new string[] { cno_user.Email }, "Locum Response Application", econtent);
        //         _emailSender.SendEmail(message);
        //         // return Ok(requestData);
        //         return Ok(approvalEntity);

        //     }


        //     return BadRequest("Invalid employee type");

        // }


        [HttpPost("updaterequest")]
public async Task<IActionResult> UpdateApprovalandRequest([FromBody] Approval approvalEntity)
{
    if (approvalEntity == null)
    {
        return BadRequest();
    }

    int t1 = (approvalEntity.Approved_By_User_Id == 0) ? 1 : 0;
    var approvalRequest = _requestFPRepository.GetRequestFormPatientById(approvalEntity.Approval_Request_Id);
    var request = _requestMainRepository.GetApprovalDetailsByRequestId(approvalEntity.Approval_Request_Id);
    var initiator = _userRepository.GetUser(request.entered_By_User_Id);
    approvalEntity.Approved_By_User_Id = approvalEntity.userId;
    var info = _userRolesRepository.GetEmployeeTypeFromUserId(approvalEntity.Approved_By_User_Id);
    var employeeType = _emptypRepository.GetEmpTypeById(info.Employee_Type_Id);
    var info_user = _userRepository.GetUser(info.User_Id);

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

        await SendCNOApprovalEmailAsync(info_user, initiator);

        return Ok(approvalEntity);
    }
    else if (employeeType.type_Name == "Supervisor")
    {
        approvalEntity.Status = "Awaiting CNO approval";
        _approvalRepository.UpdateApproval(approvalEntity);
        approvalRequest.Approval_Level_Id = employeeType.Approval_Level;
        _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
        var approvalRequests = _requestMainRepository.GetRequestsByRequestId(approvalEntity.Approval_Request_Id);
        var cno = _userRolesRepository.GetUserRoleByEmpId(3);
        var cno_user = _userRepository.GetUser(cno.User_Id);
        foreach (var req in approvalRequests)
        {
            req.Status = approvalEntity.Status;
            _requestMainRepository.UpdateRequest(req);
        }

        await SendSupervisorApprovalEmailAsync(info_user, cno_user, initiator);

        return Ok(approvalEntity);
    }

    return BadRequest("Invalid employee type");
}

private async Task SendCNOApprovalEmailAsync(User info_user, User initiator)
{
    // Email sending logic for CNO approval
    EmailSender _emailSender = new EmailSender(this._emailConfig);
    Email em = new Email();
    string logourl = "";//"https://evercaregroup.com/wp-content/uploads/2020/12/EVERCARE_LOGO_03_LEKKI_PRI_FC_RGB.png";
    string applink = "https://cafeteria.evercare.ng";
    string salutation = "Hello" + initiator.First_Name + " " + initiator.Last_Name + ",";
    string emailcontent = "Your locum payment request have been approved by " + info_user.First_Name + ' ' + info_user.Last_Name +  " Click the link below to review";
    string narration1 = " ";
    string econtent = em.HtmlMail("CNO Approval Response", applink, salutation, emailcontent, narration1, logourl);
    var message = new Message(new string[] { info_user.Email }, new string[] { initiator.Email }, "Locum Final Response Application", econtent);
    await _emailSender.SendEmailAsync(message);
}

private async Task SendSupervisorApprovalEmailAsync(User info_user, User cno_user, User initiator)
{
    // Email sending logic for Supervisor approval
    EmailSender _emailSender = new EmailSender(this._emailConfig);
    Email em = new Email();
    string logourl = "";//"https://evercaregroup.com/wp-content/uploads/2020/12/EVERCARE_LOGO_03_LEKKI_PRI_FC_RGB.png";
    string applink = "https://cafeteria.evercare.ng";
    string salutation = "Dear" + " " + cno_user.First_Name + ",";
    string emailcontent =  info_user.First_Name + ' ' + info_user.Last_Name + "approved"  + initiator.First_Name + " " + initiator.Last_Name +  "request. visit link below to review";
    string narration1 = " ";
    string econtent = em.HtmlMail("Supervisor Locum Payment Approval", applink, salutation, emailcontent, narration1, logourl);
    var message = new Message(new string[] { info_user.Email }, new string[] { cno_user.Email }, "Locum Response Application", econtent);
    await _emailSender.SendEmailAsync(message);
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
        // [HttpPost("updatedecline")]
        // public IActionResult UpdateDeclineandRequest([FromBody] Approval approvalEntity)
        // {
        //     if (approvalEntity == null)
        //     {
        //         return BadRequest();
        //     }
        //     // approvalEntity.Approval_Request_Id = approvalEntity.Id;
        //     int t1;
        //     if (approvalEntity.Approved_By_User_Id == 0)
        //     {
        //         t1 = 1;
        //     }
        //     else
        //     {
        //         t1 = 0;
        //     }
        //     var approvalRequest = _requestFPRepository.GetRequestFormPatientById(approvalEntity.Approval_Request_Id);
        //     approvalEntity.Approved_By_User_Id = approvalEntity.userId;
        //     var info = _userRolesRepository.GetEmployeeTypeFromUserId(approvalEntity.Approved_By_User_Id);
        //     var employeeType = _emptypRepository.GetEmpTypeById(info.Employee_Type_Id);
        //     //approvalEntity.Approval_Request_Id = approvalEntity.Id;
        //     approvalEntity.Status = "Declined by" + ' ' + employeeType.type_Name;

        //     if (employeeType.type_Name == "CNO")
        //     {


        //         if (t1 == 1)
        //         {
        //             _approvalRepository.UpdateApproval(approvalEntity);
        //         }
        //         else
        //         {
        //             _approvalRepository.InsertApproval(approvalEntity);
        //         }

        //         approvalRequest.Approval_Level_Id = 0;
        //         _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
        //         var approvalRequests = _requestMainRepository.GetRequestsByRequestId(approvalEntity.Approval_Request_Id);
        //         foreach (var req in approvalRequests)
        //         {
        //             req.Status = approvalEntity.Status;
        //             _requestMainRepository.UpdateRequest(req);
        //         }
        //         return Ok(approvalEntity);

        //     }
        //     else if (employeeType.type_Name == "Supervisor")
        //     {
        //         _approvalRepository.UpdateApproval(approvalEntity);
        //         approvalRequest.Approval_Level_Id = 0;
        //         _requestFPRepository.UpdateRequestFormPatient(approvalRequest);
        //         var approvalRequests = _requestMainRepository.GetRequestsByRequestId(approvalEntity.Approval_Request_Id);
        //         foreach (var req in approvalRequests)
        //         {
        //             req.Status = approvalEntity.Status;
        //             _requestMainRepository.UpdateRequest(req);
        //         }
        //         return Ok(approvalEntity);

        //     }


        //     return BadRequest("Invalid employee type");

        // }


        [HttpPost("updatedecline")]
public async Task<IActionResult> UpdateDeclineandRequest([FromBody] Approval approvalEntity)
{
    if (approvalEntity == null)
    {
        return BadRequest();
    }

    int t1 = (approvalEntity.Approved_By_User_Id == 0) ? 1 : 0;
    var approvalRequest = _requestFPRepository.GetRequestFormPatientById(approvalEntity.Approval_Request_Id);
    approvalEntity.Approved_By_User_Id = approvalEntity.userId;
    var info = _userRolesRepository.GetEmployeeTypeFromUserId(approvalEntity.Approved_By_User_Id);
    var employeeType = _emptypRepository.GetEmpTypeById(info.Employee_Type_Id);
    var info_user = _userRepository.GetUser(info.User_Id);
    approvalEntity.Status = "Declined by " + employeeType.type_Name;
    var request = _requestMainRepository.GetApprovalDetailsByRequestId(approvalEntity.Approval_Request_Id);
    var initiator = _userRepository.GetUser(request.entered_By_User_Id);
   


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

        await SendDeclineEmailAsync(info_user, initiator);

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

        await SendDeclineEmailAsync(info_user, initiator);

        return Ok(approvalEntity);
    }

    return BadRequest("Invalid employee type");
}

private async Task SendDeclineEmailAsync(User info_user, User initiator)
{
    EmailSender _emailSender = new EmailSender(this._emailConfig);
    Email em = new Email();
    string logourl = "";//"https://evercaregroup.com/wp-content/uploads/2020/12/EVERCARE_LOGO_03_LEKKI_PRI_FC_RGB.png";
    string applink = "https://cafeteria.evercare.ng";
    string salutation = "Hello" + ",";
    string emailcontent = "Your request has been declined. Please visit the link below to review.";
    string narration1 = " ";
    string econtent = em.HtmlMail("Locum Payment Declined", applink, salutation, emailcontent, narration1, logourl);
    var message = new Message(new string[] { initiator.Email }, new string[] { info_user.Email}, "Locum Payment Declined", econtent);
    await _emailSender.SendEmailAsync(message);
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

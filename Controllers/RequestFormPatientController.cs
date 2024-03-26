using Microsoft.AspNetCore.Mvc;
using Locum_Backend.Models; // Make sure to include the necessary models
using Locum_Backend.Repositories;
using Org.BouncyCastle.Crypto.Agreement.Kdf; // Make sure to include the necessary repositories



namespace Locum_Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RequestFormPatientController : ControllerBase
    {
        private readonly RequestFormPatientRepository _requestFormPatientRepository;
        private readonly ApprovalRequestRepository _requestMainRepository;
        private readonly UsersRolesRepository _userRepository;
        private readonly UserRepository _usersRepository;


        private readonly EmployeeTypeRepository _employeeTypeRepository;
        private readonly ApprovalRepository _approvalRepository;
        private readonly WardSupervisorRepository _wardsupervisorRepository;


        private EmailConfiguration _emailConfig;


        public RequestFormPatientController(UserRepository usersRepository, WardSupervisorRepository wardsupervisorRepository, EmployeeTypeRepository employeeTypeRepository, UsersRolesRepository userRepository, RequestFormPatientRepository requestFormPatientRepository, ApprovalRequestRepository requestMainRepository, ApprovalRepository approvalRepository, EmailConfiguration emailConfig)
        {
            _requestFormPatientRepository = requestFormPatientRepository;
            _requestMainRepository = requestMainRepository;
            _userRepository = userRepository;
            _employeeTypeRepository = employeeTypeRepository;
            _approvalRepository = approvalRepository;
            this._emailConfig = emailConfig;
            _wardsupervisorRepository = wardsupervisorRepository;
            _usersRepository = usersRepository;
        }

        // [HttpPost]
        // public IActionResult InsertRequestFormPatient(RequestFormPatient requestFormPatient)
        // {
        //     if (requestFormPatient == null)
        //     {
        //         return BadRequest();
        //     }
        //     else
        //     {
        //         var info = _userRepository.GetEmployeeTypeFromUserId(requestFormPatient.userId);
        //         var employeeType = _employeeTypeRepository.GetEmpTypeById(info.Employee_Type_Id);
        //         var wardsupo = _wardsupervisorRepository.GetSupervisorByWardId(requestFormPatient.ward_id);
        //         var supo = _usersRepository.GetUser(wardsupo.user_id);
        //         var info_user = _usersRepository.GetUser(info.User_Id);

        //         requestFormPatient.Is_Submitted = true;
        //         // Check the latest Approval_Request_Id
        //         var latestApprovalRequestId = _requestFormPatientRepository.GetLatestApprovalRequestId();

        //         // Set the Approval_Request_Id
        //         requestFormPatient.Approval_Request_Id = latestApprovalRequestId == null || latestApprovalRequestId == 0
        //         ? 10001
        //         : latestApprovalRequestId + 1;


        //         requestFormPatient.Approval_Level_Id = employeeType.Approval_Level;


        //         _requestFormPatientRepository.InsertRequestFormPatient(requestFormPatient);
        //         var req = _requestMainRepository.GetRequestsByUserIdFromView(requestFormPatient.userId);
        //         foreach (var requestItem in req)
        //         {
        //             var reqq = _requestMainRepository.GetRequestById(requestItem.Id);
        //             reqq.Date_Entered = DateTime.Now;
        //             reqq.Created_At = DateTime.Now;
        //             reqq.request_Id = _requestFormPatientRepository.GetLatestApprovalRequestId();
        //             reqq.request_code = "RE00" + reqq.request_Id.ToString();
        //             _requestMainRepository.UpdateRequest(reqq);
        //         }
        //         var approval = new Approval();
        //         approval.Status = "Pending with Supervisor";
        //         approval.Approval_Request_Id = _requestFormPatientRepository.GetLatestApprovalRequestId();
        //         _approvalRepository.InsertApproval(approval);


        //         EmailSender _emailSender = new EmailSender(this._emailConfig);
        //         Email em = new Email();
        //         string logourl = "";//"https://evercaregroup.com/wp-content/uploads/2020/12/EVERCARE_LOGO_03_LEKKI_PRI_FC_RGB.png";
        //         string applink = "https://cafeteria.evercare.ng";
        //         string salutation = "Hello" + ",";
        //         string emailcontent = "A locum payment request has been initiated by " + info_user.First_Name + ' ' + info_user.Last_Name + " Kindly review and approve the request by clicking on the link below";
        //         string narration1 = " ";
        //         string econtent = em.HtmlMail("New Approval Request", applink, salutation, emailcontent, narration1, logourl);
        //         var message = new Message(new string[] { info_user.Email }, new string[] { supo.Email }, "Locum Application", econtent);
        //         _emailSender.SendEmail(message);
        //         // return Ok(requestData);

        //         return NoContent();
        //     }
        // }


[HttpPost]
public async Task<IActionResult> InsertRequestFormPatient(RequestFormPatient requestFormPatient)
{
    try
    {
        if (requestFormPatient == null)
        {
            return BadRequest();
        }
        else
        {
            var info = _userRepository.GetEmployeeTypeFromUserId(requestFormPatient.userId);
            var employeeType = _employeeTypeRepository.GetEmpTypeById(info.Employee_Type_Id);
            var wardsupo = _wardsupervisorRepository.GetSupervisorByWardId(requestFormPatient.ward_id);
            var supo = _usersRepository.GetUser(wardsupo.user_id);
            var info_user = _usersRepository.GetUser(info.User_Id);

            requestFormPatient.Is_Submitted = true;
            // Check the latest Approval_Request_Id
            var latestApprovalRequestId = _requestFormPatientRepository.GetLatestApprovalRequestId();

            // Set the Approval_Request_Id
            requestFormPatient.Approval_Request_Id = latestApprovalRequestId == null || latestApprovalRequestId == 0
            ? 10001
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
                reqq.request_code = "RE00" + reqq.request_Id.ToString();
                _requestMainRepository.UpdateRequest(reqq);
            }
            var approval = new Approval();
            approval.Status = "Pending with Supervisor";
            approval.Approval_Request_Id = _requestFormPatientRepository.GetLatestApprovalRequestId();
            _approvalRepository.InsertApproval(approval);

            // Send email asynchronously
            await SendApprovalRequestEmailAsync(info_user, supo);

            return NoContent();
        }
    }
    catch (Exception ex)
    {
        // Log or handle the exception as needed
        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
    }
}

private async Task SendApprovalRequestEmailAsync(User info_user, User supo)
{
    EmailSender _emailSender = new EmailSender(this._emailConfig);
    Email em = new Email();
    string logourl = "";//"https://evercaregroup.com/wp-content/uploads/2020/12/EVERCARE_LOGO_03_LEKKI_PRI_FC_RGB.png";
    string applink = "https://cafeteria.evercare.ng";
    string salutation = "Hello" + ",";
    string emailcontent = "A locum payment request has been initiated by " + info_user.First_Name + ' ' + info_user.Last_Name + " Kindly review and approve the request by clicking on the link below";
    string narration1 = " ";
    string econtent = em.HtmlMail("Locum Payment Approval", applink, salutation, emailcontent, narration1, logourl);
    var message = new Message(new string[] { info_user.Email }, new string[] { supo.Email }, "Locum Application", econtent);
    await _emailSender.SendEmailAsync(message);
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
}
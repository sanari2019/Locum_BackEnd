using System;
using RepoDb.Attributes;



namespace Locum_Backend.Models
{
    [Map("[approvals]")]
    public class Approval
    {
        public int Id { get; set; }
        public int Approval_Request_Id { get; set; }
        public int Approved_By_User_Id { get; set; }
        public string? Status { get; set; }
        public int userId { get; set; }
        public string? Decline_Reason { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? ApprovedByUser { get; set; }
        public ApprovalRequest? ApprovalRequest { get; set; }
    }
}

using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{
    [Map("[request_form_patients]")]
    public class RequestFormPatient
    {
        [Primary]
        public int Approval_Request_Id { get; set; }
        public int Approval_Level_Id { get; set; }
        public bool Is_Submitted { get; set; }
        public string? first_Name { get; set; }
        public string? last_Name { get; set; }


        public int userId { get; set; }
        public ApprovalRequest? approvalRequest { get; set; }
        public Approval? approval { get; set; }
        public Department? department { get; set; }
        public User? user { get; set; }
        public Shift? shift { get; set; }
        public ApprovalDetails? approvalDetails { get; set; }
        public Patient? patient { get; set; }

    }
}

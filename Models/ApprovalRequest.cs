using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{
    [Map("[approval_requests]")]
    public class ApprovalRequest
    {
        public int Id { get; set; }
        public DateTime Date_Entered { get; set; }
        public int Entered_By_User_Id { get; set; }
        public int Department_Id { get; set; }
        public int Shift_Id { get; set; }
        public int Patient_Id { get; set; }
        public int request_Id { get; set; }
        public required string Status { get; set; }
        public int Approval_Level { get; set; }
        public DateTime Created_At { get; set; }

        // Navigation properties
        public User? EnteredByUser { get; set; }
        public Department? Department { get; set; }
        public Shift? Shift { get; set; }

        public Patient? Patient { get; set; }
    }
}

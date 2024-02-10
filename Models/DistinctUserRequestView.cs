using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locum_Backend.Models
{
    [Table("vw_DistinctUserRequestView")]
    public class DistinctUserRequestViewModel
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int entered_By_User_Id { get; set; }

        [Required]
        public int request_Id { get; set; }
        public DateTime date_Entered { get; set; }

        public int approval_Request_Id { get; set; }

        public int approvedByUserId { get; set; }

        public string? status { get; set; }

        public string? department { get; set; }
        public string? shift { get; set; }
        public string? decline_Reason { get; set; }
    }
}

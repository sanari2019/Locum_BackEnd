using System;
using RepoDb.Attributes;
using System.ComponentModel.DataAnnotations;


namespace Locum_Backend.Models
{


    public class NursedPatient
    {
        [Required(ErrorMessage = "The UHID field is required.")]
        public string  UHID { get; set; }
        public string?  RoomNo { get; set; }
        public string?  WardName { get; set; }
        public DateTime DateNursed { get; set; }
        public int ShiftId { get; set; }
    }
}
using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{

    public class ValidatedPatient
    {

        public string Patient { get; set; }
        public string RegistrationNo { get; set; }
        public string WardName { get; set; }
        public string BedCategory { get; set; }
        public string RoomNo { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
    }
}

using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{
    [Map("[patients]")]
    public class Patient
    {
        public int Id { get; set; }
        public required string first_Name { get; set; }
        public required string last_Name { get; set; }
        public required string uhid { get; set; }
        public bool Is_Validated { get; set; }
        public DateTime Created_At { get; set; }
    }
}

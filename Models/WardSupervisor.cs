using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{
    [Map("[wardSupervisor]")]
    public class WardSupervisor
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int ward_id { get; set; }
        public bool active { get; set; }
    }
}

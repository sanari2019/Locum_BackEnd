using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{
    [Map("[wardNames]")]
    public class WardNames
    {
        public int Id { get; set; }
        public required string ward_name { get; set; }
    }
}

using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{
    [Map("[shifts]")]
    public class Shift
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

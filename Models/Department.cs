using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{
    [Map("[departments]")]
    public class Department
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

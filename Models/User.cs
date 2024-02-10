using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{
    [Map("[users]")]
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public DateTime Created_At { get; set; }
        public required string Locum_Type { get; set; }
        public required string Phone_Number { get; set; }
        public required string First_Name { get; set; }
        public int Department_id { get; set; }
        public required string Last_Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Bank_Name { get; set; }
        public required string Holders_Name { get; set; }
        public required string Account_Number { get; set; }
        public string? Sort_Code { get; set; }
        public bool Active { get; set; }
        public bool Agree_Terms { get; set; }

    }
}

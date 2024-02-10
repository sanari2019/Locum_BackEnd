using System;
using RepoDb.Attributes;


namespace Locum_Backend.Models
{

    [Map("[employee_types]")]
    public class EmployeeType
    {
        public int Id { get; set; }
        public string? type_Name { get; set; }
        public int Approval_Level { get; set; }
    }
}
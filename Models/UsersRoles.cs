using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{
    [Map("[usersroles]")]

    public class UsersRoles
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Employee_Type_Id { get; set; }
        public DateTime Created_At { get; set; }

        // You can add navigation properties if needed
        public User? user { get; set; }
        public EmployeeType EmployeeType { get; set; } = new EmployeeType();
    }


}

// RequestFormPatientModel.cs

using System;
using System.ComponentModel.DataAnnotations;

public class ApprovalDetails
{
    [Key]
    public int id;
    public int approval_Request_Id { get; set; }
    public int approval_Level_Id { get; set; }
    public int is_Submitted { get; set; }
    public DateTime date_Entered { get; set; }
    public int entered_By_User_Id { get; set; }
    public int department_Id { get; set; }
    public int shift_Id { get; set; }
    public string status { get; set; }

    public int patient_Id { get; set; }
    public string locum_Type { get; set; }
    public string first_Name { get; set; }
    public string last_Name { get; set; }
    public string department { get; set; }
    public string shift_Name { get; set; }
}

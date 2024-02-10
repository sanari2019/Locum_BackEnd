using System.Collections.Generic;
using System.Linq;
using RepoDb;
using RepoDb.DbSettings;
using RepoDb.Enumerations;
using RepoDb.StatementBuilders;
using RepoDb.DbHelpers;
using Microsoft.Data.SqlClient;
using Locum_Backend;
using Locum_Backend.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Locum_Backend.Repositories // Adjust the namespace as per your project structure
{

    public class UserRepository : BaseRepository<User, SqlConnection>
    {
        Setting sett = new Setting();

        public UserRepository(Setting sett) : base(sett.ConnectionStrings)
        {
            this.sett = sett;
            DbSettingMapper.Add<SqlConnection>(new SqlServerDbSetting(), true);
            DbHelperMapper.Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper.Add<SqlConnection>(new SqlServerStatementBuilder(new SqlServerDbSetting()), true);
        }

        public void InsertUser(User user)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Insert(user);
            }
        }

        public void UpdateUser(User user)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Update(user);
            }
        }

        public int DeleteUser(User user)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                return connection.Delete<User>(user);
            }
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {

                users = connection.QueryAll<User>().ToList();
            }
            return users;
        }

        public User GetUser(int id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<User>(id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
        public User GetUserByEmail(string email)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                // Assuming there is an Email property in the User model
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<User>(e => e.Email == email).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
        public User GetUserByUsername(string username)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                // Assuming there is an Email property in the User model
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<User>(e => e.Username == username).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }








    }


    public class UsersRolesRepository : BaseRepository<User, SqlConnection>
    {
        Setting sett = new Setting();

        public UsersRolesRepository(Setting sett) : base(sett.ConnectionStrings)
        {
            this.sett = sett;
            DbSettingMapper.Add<SqlConnection>(new SqlServerDbSetting(), true);
            DbHelperMapper.Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper.Add<SqlConnection>(new SqlServerStatementBuilder(new SqlServerDbSetting()), true);
        }

        public void InsertUserRole(UsersRoles usersRoles)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Insert(usersRoles);
            }
        }

        public void UpdateUserRole(UsersRoles usersRoles)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Update(usersRoles);
            }
        }

        public int DeleteUserRole(UsersRoles usersRoles)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                return connection.Delete<User>(usersRoles);
            }
        }

        public List<UsersRoles> GetUserRoles()
        {
            var userRoles = new List<UsersRoles>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                userRoles = connection.QueryAll<UsersRoles>().ToList();
            }
            return userRoles;
        }

        public UsersRoles GetUserRoleById(int id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<UsersRoles>(id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
        public UsersRoles GetUserRoleByUserId(int user_id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                var sql = "SELECT * FROM usersroles WHERE User_Id = @user_id";
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<UsersRoles>(sql, new { user_id }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        public List<UsersRoles> GetUserRolesByUserIdFromView(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    return connection.ExecuteQuery<UsersRoles>(
                        "getUserRolesByUserIdFromView",
                        new { UserId = userId },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting user roles from view: {ex.Message}", ex);
            }
        }
        public UsersRoles GetEmployeeTypeFromUserId(int user_Id)
        {
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return connection.ExecuteQuery<UsersRoles>(
                    "getEmployeeTypeFromUserId",
                    new { user_Id = user_Id },
                    commandType: CommandType.StoredProcedure
                ).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting employee type from user: {ex.Message}", ex);
            }
        }
    }








    public class DepartmentRepository : BaseRepository<Department, SqlConnection>
    {
        Setting sett = new Setting();

        public DepartmentRepository(Setting sett) : base(sett.ConnectionStrings)
        {
            this.sett = sett;
            DbSettingMapper.Add<SqlConnection>(new SqlServerDbSetting(), true);
            DbHelperMapper.Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper.Add<SqlConnection>(new SqlServerStatementBuilder(new SqlServerDbSetting()), true);
        }

        public void InsertDepartment(Department department)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Insert(department);
            }
        }

        public List<Department> GetDepartments()
        {
            var departments = new List<Department>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                departments = connection.QueryAll<Department>().ToList();
            }
            return departments;
        }

        public Department GetDepartmentById(int id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<Department>(id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
    }

    public class ShiftRepository : BaseRepository<Shift, SqlConnection>
    {
        Setting sett = new Setting();

        public ShiftRepository(Setting sett) : base(sett.ConnectionStrings)
        {
            this.sett = sett;
            DbSettingMapper.Add<SqlConnection>(new SqlServerDbSetting(), true);
            DbHelperMapper.Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper.Add<SqlConnection>(new SqlServerStatementBuilder(new SqlServerDbSetting()), true);
        }

        public void InsertShift(Shift shift)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Insert(shift);
            }
        }

        public List<Shift> GetShifts()
        {
            var shifts = new List<Shift>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                shifts = connection.QueryAll<Shift>().ToList();
            }
            return shifts;
        }

        public Shift GetShiftById(int id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<Shift>(id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
    }

    public class EmployeeTypeRepository : BaseRepository<EmployeeType, SqlConnection>
    {
        Setting sett = new Setting();

        public EmployeeTypeRepository(Setting sett) : base(sett.ConnectionStrings)
        {
            this.sett = sett;
            DbSettingMapper.Add<SqlConnection>(new SqlServerDbSetting(), true);
            DbHelperMapper.Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper.Add<SqlConnection>(new SqlServerStatementBuilder(new SqlServerDbSetting()), true);
        }

        public void InsertEmpType(EmployeeType empType)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Insert(empType);
            }
        }

        public List<EmployeeType> GetEmpTypes()
        {
            var emptypes = new List<EmployeeType>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                emptypes = connection.QueryAll<EmployeeType>().ToList();
            }
            return emptypes;
        }

        public EmployeeType GetEmpTypeById(int id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<EmployeeType>(id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
    }

    public class ApprovalRequestRepository : BaseRepository<ApprovalRequest, SqlConnection>
    {
        Setting sett = new Setting();

        public ApprovalRequestRepository(Setting sett) : base(sett.ConnectionStrings)
        {
            this.sett = sett;
            DbSettingMapper.Add<SqlConnection>(new SqlServerDbSetting(), true);
            DbHelperMapper.Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper.Add<SqlConnection>(new SqlServerStatementBuilder(new SqlServerDbSetting()), true);
        }


        public void InsertRequest(ApprovalRequest request)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Insert(request);
            }
        }

        public ApprovalDetails GetApprovalDetailsByRequestId(int approvalRequestId)
        {
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    return connection.ExecuteQuery<ApprovalDetails>(
                                                "[dbo].[usp_getApprovalDetails]",
                            new { Approval_Request_Id = approvalRequestId },
                            commandType: CommandType.StoredProcedure
                        ).FirstOrDefault();
                    // connection.Query<ApprovalDetails>(e => e.ApprovalRequestId == approvalRequestId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting approval details by request id: {ex.Message}", ex);
            }
        }




        public List<ApprovalDetails> GetApprovalDetailssByRequestId(int approval_Request_Id)
        {
            var pendingDetails = new List<ApprovalDetails>();
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    // Assuming getpendingrequestsbyuserid is the stored procedure
                    pendingDetails = connection.ExecuteQuery<ApprovalDetails>(
    "[dbo].[usp_getApprovalDetails]",
    new { Approval_Request_Id = approval_Request_Id },
    commandType: CommandType.StoredProcedure
).ToList();


                }
                return pendingDetails;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting approval details by request id: {ex.Message}", ex);
            }
        }


        public List<ApprovalRequest> GetRequests()
        {
            var requestMain = new List<ApprovalRequest>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                requestMain = connection.QueryAll<ApprovalRequest>().ToList();
            }
            return requestMain;
        }

        public ApprovalRequest GetRequestById(int id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<ApprovalRequest>(id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
        public ApprovalRequest GetRequestByPatientId(int patient_id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<ApprovalRequest>(e => e.Patient_Id == patient_id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
        public ApprovalRequest GetRequestByRequestId(int request_id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<ApprovalRequest>(e => e.request_Id == request_id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
        public List<ApprovalRequest> GetRequestsByRequestId(int request_id)
        {
            var requests = new List<ApprovalRequest>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                requests = connection.Query<ApprovalRequest>(e => e.request_Id == request_id).ToList();
                foreach (ApprovalRequest sv in requests)
                {
                    // sv.User = connection.Query<User>(sv.EnteredBy).FirstOrDefault();
                    // sv.Department = connection.Query<Department>(sv.DepartmentId).FirstOrDefault();
                    sv.Patient = connection.Query<Patient>(sv.Patient_Id).FirstOrDefault();
                    // var userRequests = GetRequestsByUserId(EnteredBy);

                }
            }
            return requests;
        }

        public void UpdateRequest(ApprovalRequest request)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Update(request);
            }
        }

        public int DeleteRequest(ApprovalRequest request)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                return connection.Delete<ApprovalRequest>(request);
            }
        }

        public List<ApprovalRequest> GetRequestsByUserId(int userId)
        {
            var requests = new List<ApprovalRequest>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                requests = connection.Query<ApprovalRequest>(e => e.Id == userId).ToList();

            }
            return requests;
        }
        public List<ApprovalRequest> GetApprovedRequests()
        {
            try
            {
                var requests = new List<ApprovalRequest>();
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    requests = connection.Query<ApprovalRequest>("GetApprovedRequests").ToList();
                }
                return requests;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting approved requests: {ex.Message}", ex);
            }
        }


        public List<ApprovalRequest> GetPendingRequestsByUserId(int EnteredBy)
        {
            var pendingRequests = new List<ApprovalRequest>();
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    // Assuming getpendingrequestsbyuserid is the stored procedure
                    pendingRequests = connection.ExecuteQuery<ApprovalRequest>("[dbo].[usp_getPendingRequestsByUserId]", new { EnteredBy = EnteredBy },
                          commandType: System.Data.CommandType.StoredProcedure
                      ).ToList();

                    foreach (ApprovalRequest sv in pendingRequests)
                    {
                        // sv.User = connection.Query<User>(sv.EnteredBy).FirstOrDefault();
                        // sv.Department = connection.Query<Department>(sv.DepartmentId).FirstOrDefault();
                        // sv.Shift = connection.Query<Shift>(sv.ShiftId).FirstOrDefault();
                        // var userRequests = GetRequestsByUserId(EnteredBy);
                        // sv.PatientNames = userRequests.Select(r => r.PatientFirstName).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting pending requests: {ex.Message}", ex);
            }
            return pendingRequests;
        }


        public List<ApprovalRequest> GetDecidedRequestsByUserId(int EnteredBy)
        {
            var decidedRequests = new List<ApprovalRequest>();
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    // Assuming getpendingrequestsbyuserid is the stored procedure
                    decidedRequests = connection.ExecuteQuery<ApprovalRequest>("[dbo].[usp_getDecidedRequestsByUserId]", new { EnteredBy = EnteredBy },
                          commandType: System.Data.CommandType.StoredProcedure
                      ).ToList();
                    foreach (ApprovalRequest sv in decidedRequests)
                    {
                        // sv.User = connection.Query<User>(sv.EnteredBy).FirstOrDefault();
                        // sv.Department = connection.Query<Department>(sv.DepartmentId).FirstOrDefault();
                        // sv.Shift = connection.Query<Shift>(sv.ShiftId).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting pending requests: {ex.Message}", ex);
            }
            return decidedRequests;
        }



        public List<RequestFormPatient> GetCnoPendingRequests()
        {
            var decidedRequests = new List<RequestFormPatient>();
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    // Assuming getpendingrequestsbyuserid is the stored procedure
                    decidedRequests = connection.ExecuteQuery<RequestFormPatient>("[dbo].[usp_getCnoPendingRequests]",
                          commandType: System.Data.CommandType.StoredProcedure
                      ).ToList();
                    foreach (RequestFormPatient sv in decidedRequests)
                    {
                        // sv.User = connection.Query<User>(sv.EnteredBy).FirstOrDefault();
                        sv.approvalDetails = connection.ExecuteQuery<ApprovalDetails>(
                                           "[dbo].[usp_getApprovalDetails]",
                       new { Approval_Request_Id = sv.Approval_Request_Id },
                       commandType: CommandType.StoredProcedure
                   ).FirstOrDefault();
                        sv.approval = connection.ExecuteQuery<Approval>(
                                                "SELECT * FROM [Locum].[dbo].[approvals] Where approval_request_id = @Approval_Request_Id",
                            new { Approval_Request_Id = sv.Approval_Request_Id }
                        ).FirstOrDefault();
                        //         var query = "SELECT * FROM request_Form_Patients WHERE approval_request_id = @Approval_Request_Id";
                        // var parameters = new { Approval_Request_Id = approvalRequestId };
                        // return connection.ExecuteQuery<RequestFormPatient>(query, parameters).FirstOrDefault();

                        sv.department = connection.Query<Department>(sv.approvalDetails.department_Id).FirstOrDefault();
                        sv.user = connection.Query<User>(sv.approvalDetails.entered_By_User_Id).FirstOrDefault();
                        sv.shift = connection.Query<Shift>(sv.approvalDetails.shift_Id).FirstOrDefault();
                        sv.patient = connection.Query<Patient>(sv.approvalDetails.patient_Id).FirstOrDefault();
                        // sv.Shift = connection.Query<Shift>(sv.ShiftId).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting pending requests: {ex.Message}", ex);
            }
            return decidedRequests;
        }

        public List<RequestFormPatient> GetSupApprovalRequests()
        {
            var decidedRequests = new List<RequestFormPatient>();
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    // Assuming getpendingrequestsbyuserid is the stored procedure
                    decidedRequests = connection.ExecuteQuery<RequestFormPatient>("[dbo].[usp_getSupPendingRequests]",
                          commandType: System.Data.CommandType.StoredProcedure
                      ).ToList();
                    foreach (RequestFormPatient sv in decidedRequests)
                    {

                        // #pragma warning disable CS8601 // Possible null reference assignment.
                        //                         sv.approvalRequest = connection.ExecuteQuery<ApprovalRequest>(
                        //                         "[dbo].[usp_getApprovalDetails]",
                        //     new { Approval_Request_Id = sv.Approval_Request_Id },
                        //     commandType: CommandType.StoredProcedure
                        // ).FirstOrDefault();
                        // #pragma warning restore CS8601 // Possible null reference assignment.
                        sv.approvalDetails = connection.ExecuteQuery<ApprovalDetails>(
                                                "[dbo].[usp_getApprovalDetails]",
                            new { Approval_Request_Id = sv.Approval_Request_Id },
                            commandType: CommandType.StoredProcedure
                        ).FirstOrDefault();
                        sv.approval = connection.ExecuteQuery<Approval>(
                                                "SELECT * FROM [Locum].[dbo].[approvals] Where approval_request_id = @Approval_Request_Id",
                            new { Approval_Request_Id = sv.Approval_Request_Id }
                        ).FirstOrDefault();
                        //         var query = "SELECT * FROM request_Form_Patients WHERE approval_request_id = @Approval_Request_Id";
                        // var parameters = new { Approval_Request_Id = approvalRequestId };
                        // return connection.ExecuteQuery<RequestFormPatient>(query, parameters).FirstOrDefault();

                        sv.department = connection.Query<Department>(sv.approvalDetails.department_Id).FirstOrDefault();
                        sv.user = connection.Query<User>(sv.approvalDetails.entered_By_User_Id).FirstOrDefault();
                        sv.shift = connection.Query<Shift>(sv.approvalDetails.shift_Id).FirstOrDefault();
                        sv.patient = connection.Query<Patient>(sv.approvalDetails.patient_Id).FirstOrDefault();


                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting pending requests: {ex.Message}", ex);
            }
            return decidedRequests;
        }

        public List<ApprovalRequest> GetRequestsByUserIdFromView(int enteredByUserId)
        {
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    return connection.ExecuteQuery<ApprovalRequest>(
                        "[dbo].[usp_getRequestsByUserIdfromReq]",
                        new { entered_by_user_id = enteredByUserId },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting requests by user from view: {ex.Message}", ex);
            }
        }




    }


    public class ApprovalRepository : BaseRepository<Approval, SqlConnection>
    {
        Setting sett = new Setting();

        public ApprovalRepository(Setting sett) : base(sett.ConnectionStrings)
        {
            this.sett = sett;
            DbSettingMapper.Add<SqlConnection>(new SqlServerDbSetting(), true);
            DbHelperMapper.Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper.Add<SqlConnection>(new SqlServerStatementBuilder(new SqlServerDbSetting()), true);
        }

        public void InsertApproval(Approval approvalEntity)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Insert(approvalEntity);
            }
        }
        public void UpdateApproval(Approval approvalEntity)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Update(approvalEntity);
            }
        }


        public List<Approval> GetApprovals()
        {
            var approvals = new List<Approval>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                approvals = connection.QueryAll<Approval>().ToList();
            }
            return approvals;
        }

        public Approval GetApprovalById(int id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<Approval>(id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
        public List<DistinctUserRequestViewModel> GetApprovalDetailsByUserId(int enteredByUserId)
        {
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    return connection.ExecuteQuery<DistinctUserRequestViewModel>(
                                                "SELECT TOP (1000) * FROM [Locum].[dbo].[vw_DistinctUserRequestView] where entered_by_user_id=@entered_by_user_id",
                            new { entered_by_user_id = enteredByUserId }
                        ).ToList();
                    // connection.Query<ApprovalDetails>(e => e.ApprovalRequestId == approvalRequestId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting approval details by request id: {ex.Message}", ex);
            }
        }

        // Add more methods as needed for approval handling
    }

    public class PatientRepository : BaseRepository<Patient, SqlConnection>
    {
        Setting sett = new Setting();

        public PatientRepository(Setting sett) : base(sett.ConnectionStrings)
        {
            this.sett = sett;
            DbSettingMapper.Add<SqlConnection>(new SqlServerDbSetting(), true);
            DbHelperMapper.Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper.Add<SqlConnection>(new SqlServerStatementBuilder(new SqlServerDbSetting()), true);
        }

        public void InsertPatient(Patient patient)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Insert(patient);
            }
        }

        public void UpdatePatient(Patient patient)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Update(patient);
            }
        }

        public int DeletePatient(Patient patient)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                return connection.Delete<Patient>(patient);
            }
        }

        public List<Patient> GetPatients()
        {
            var patients = new List<Patient>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                patients = connection.QueryAll<Patient>().ToList();
            }
            return patients;
        }

        public Patient GetPatientById(int id)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<Patient>(id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        public List<Patient> GetValidatedPatientByUser(int enteredByUserId)
        {
            try
            {
                using (var connection = new SqlConnection(sett.ConnectionStrings))
                {
                    return connection.ExecuteQuery<Patient>("usp_getValidatedPatientByUser", new { entered_by_user_id = enteredByUserId },
                          commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new ApplicationException($"Error getting validated patients by user: {ex.Message}", ex);
            }
        }
        public Patient GetValidatedPatientByIDandUserID(int patientId, int requestId)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                var result = connection
                    .ExecuteQuery<Patient>(
                        "usp_getValidatedPatientByIDandUserID",
                        new { request_id = requestId, patient_id = patientId },
                        commandType: CommandType.StoredProcedure
                    )
                    .FirstOrDefault();
                if (result == null)
                {

                }
#pragma warning disable CS8603 // Possible null reference return.
                return result;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }




    }
    public class RequestFormPatientRepository : BaseRepository<RequestFormPatient, SqlConnection>
    {
        Setting sett = new Setting();

        public RequestFormPatientRepository(Setting sett) : base(sett.ConnectionStrings)
        {
            this.sett = sett;
            DbSettingMapper.Add<SqlConnection>(new SqlServerDbSetting(), true);
            DbHelperMapper.Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper.Add<SqlConnection>(new SqlServerStatementBuilder(new SqlServerDbSetting()), true);
        }

        public void InsertRequestFormPatient(RequestFormPatient requestFormPatient)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Insert(requestFormPatient);
            }
        }

        public void UpdateRequestFormPatient(RequestFormPatient requestFormPatient)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                connection.Update(requestFormPatient);
            }
        }

        public void UpdateRequestFormPatientApprovalLevel(RequestFormPatient requestFormPatient)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                // var query = "Update request_form_patients set approval_";
                var query = "UPDATE request_form_patients SET approval_level_Id = @Approval_Level_id WHERE approval_request_id = @Approval_Request_Id";

                // Assuming ApprovalLevel and ApprovalRequestId are properties in your RequestFormPatient class
                var parameters = new
                {
                    Approval_Level_Id = requestFormPatient.Approval_Level_Id,
                    Approval_Request_Id = requestFormPatient.Approval_Request_Id
                };

                connection.ExecuteQuery<RequestFormPatient>(query, parameters);
                // connection.Update(requestFormPatient);
            }
        }

        public int DeleteRequestFormPatient(RequestFormPatient requestFormPatient)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                return connection.Delete<RequestFormPatient>(requestFormPatient);
            }
        }

        public List<RequestFormPatient> GetRequestFormPatients()
        {
            var requestFormPatients = new List<RequestFormPatient>();
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                requestFormPatients = connection.QueryAll<RequestFormPatient>().ToList();
            }
            return requestFormPatients;
        }

        public RequestFormPatient GetRequestFormPatientById(int approvalRequestId)
        {
            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {
                var query = "SELECT * FROM request_Form_Patients WHERE approval_request_id = @Approval_Request_Id";
                var parameters = new { Approval_Request_Id = approvalRequestId };
                return connection.ExecuteQuery<RequestFormPatient>(query, parameters).FirstOrDefault();

            }
        }

        public int GetLatestApprovalRequestId()
        {

            using (var connection = new SqlConnection(sett.ConnectionStrings))
            {

                var query = "SELECT MAX(Approval_Request_Id)   FROM [Locum].[dbo].[request_form_patients]";
                return connection.ExecuteQuery<int>(query).FirstOrDefault();
            }
        }
    }



}

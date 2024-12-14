using NexusEcom.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.DataAccess.Repositories.Interfaces
{
    public interface ILeaveRepository
    {
        Task<LeaveBalance> GetLeaveBalanceByEmployeeIdAsync(string employeeId); // View Current Leave Balance
        Task AddLeaveRequestAsync(Leave leave); // Submit Leave Request
        Task<List<Leave>> GetLeaveRequestsByEmployeeIdAsync(string employeeId); // Track Leave Balances
        Task<List<Leave>> GetAllLeaveRequestsAsync(); // HR Admin: View all leave requests
        Task<Leave> GetLeaveRequestByIdAsync(int leaveId); // HR Admin: View a specific leave request
        Task UpdateLeaveStatusAsync(int leaveId, string status, string comments); // Approve/Reject Leave
        Task<List<Leave>> GetLeavesByCriteriaAsync(DateTime startDate, DateTime endDate, string? employeeId, int? leaveTypeId); // Generate Leave Reports
    }
}

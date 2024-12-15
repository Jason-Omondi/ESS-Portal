

using NexusEcom.Data.DataTransferObjects;

namespace NexusEcom.Controllers.Services.Interfaces
{
    public interface ILeaveService
    {
        Task<LeaveBalanceDto> GetLeaveBalanceAsync(string employeeId);
        Task<List<LeaveResponseDto>> GetLeaveRequestsByEmployeeIdAsync(string employeeId);
        Task SubmitLeaveRequestAsync(CreateLeaveDto leaveDto);
        Task UpdateLeaveStatusAsync(int leaveId, string status, string comments = null);
        Task<List<LeaveResponseDto>> GetAllLeaveRequestsAsync();
        Task<List<LeaveResponseDto>> GetLeavesByCriteriaAsync(DateTime startDate, DateTime endDate, string? employeeId, int? leaveTypeId);
    }
}



namespace NexusEcom.DataAccess.DataTransferObjects
{
    public class LeaveBalanceDto
    {
        public int LeaveBalanceId { get; set; }
        public string EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public int TotalEntitlementDays { get; set; }
        public int RemainingDays { get; set; }
        public int ConsumedDays { get; set; }
        public int CarriedForwardDays { get; set; }
    }

    public class CreateLeaveDto
    {
        public string EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // Uncomment if you want to track reason
        // public string Reason { get; set; }
    }

    public class LeaveResponseDto
    {
        public int LeaveId { get; set; }
        public string EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public LeaveBalanceDto LeaveBalance { get; set; }
    }
}


//public class LeaveApplicationDto
//{
//    // Leave Application Details
//    public int LeaveId { get; set; }
//    public int EmployeeId { get; set; }
//    public string LeaveType { get; set; }
//    public DateTime StartDate { get; set; }
//    public DateTime EndDate { get; set; }
//    public int TotalDays { get; set; }
//    public string Status { get; set; }
//    public DateTime CreatedAt { get; set; }
//    public string? AttachmentPath { get; set; }

//    // Leave Balance Details
//    public LeaveBalanceDetails LeaveBalance { get; set; }

//    public class LeaveBalanceDetails
//    {
//        public string LeaveType { get; set; }
//        public int TotalEntitlementDays { get; set; }
//        public int RemainingDays { get; set; }
//        public int ConsumedDays { get; set; }
//        public int? CarriedForward { get; set; }
//    }
//}
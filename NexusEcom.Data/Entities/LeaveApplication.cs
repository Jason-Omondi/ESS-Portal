
namespace NexusEcom.Data.Entities
{

    public class Leave
    {
        public int LeaveId { get; set; } // Primary Key
        public string EmployeeId { get; set; }
        public int leaveBalanceId { get; set; } //fk
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Approved"
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Balance Info (embedded for simplicity)
        public LeaveBalance LeaveBalance { get; set; }
    }

    public class LeaveBalance
    {
        public int leaveBalanceId { get; set; } // Primary Key
        public string EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public int TotalEntitlementDays { get; set; }
        public int RemainingDays { get; set; }
        public int ConsumedDays { get; set; }
        public int CarriedForwardDays { get; set; }
        public ICollection<Leave> Leaves { get; set; }
    }


 

}


//public class LeaveBalance
//{
//    public int LeaveBalId { get; set; }
//    public int EmployeeId { get; set; } // Foreign Key for Employee/User

//    public string LeaveType { get; set; } // Can later be refactored into its own table
//    public int TotalEntitlementDays { get; set; } // Changed to int for numeric consistency
//    public int RemainingDays { get; set; }
//    public int ConsumedDays { get; set; }
//    public int? CarriedForward { get; set; } // Nullable in case not applicable

//    // Navigation property for related applications
//    public ICollection<LeaveApplication>? LeaveApplications { get; set; }

//}

//public class LeaveApplication
//{

//    public int LeaveId { get; set; }
//    public int EmployeeId { get; set; } // Foreign Key to the user requesting leave

//    public string LeaveType { get; set; } // Could be an enum for better maintainability
//    public DateTime StartDate { get; set; }
//    public DateTime EndDate { get; set; }
//    public int TotalDays { get; set; }

//    public string Status { get; set; } // E.g., "Pending", "Approved", "Rejected"
//    public DateTime CreatedAt { get; set; }

//    public string? AttachmentPath { get; set; } // Optional document for the leave request

//    // Navigation property for the employee relationship
//    public LeaveBalance? LeaveBalance { get; set; }


//}

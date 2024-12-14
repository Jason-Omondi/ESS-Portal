using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.DataAccess.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // e.g., "Admin", "Employee", "HR"
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}


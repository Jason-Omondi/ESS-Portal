using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.DataAccess.Entities
{
    public class User
    {
        public int userId { get; set; }
        public string Email { get; set; }

        //public string employeeNumber { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // "Admin" or "Customer"
        public DateTime CreatedAt { get; set; }
    }
}

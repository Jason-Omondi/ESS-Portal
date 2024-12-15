using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.DataAccess.DataTransferObjects
{
    public class UserDto
    {
        //public int UserId { get; set; }
        public string Email { get; set; }
        public string EmployeeNumber { get; set; }

        public string Password {  get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}

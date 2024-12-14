using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.Data.Models
{
    public class Order
    {
        public int oderId { get; set; }        
        public int ProductId { get; set; }        
        public int Quantity { get; set; }        
        public DateTime OrderDate { get; set; }  
    }
}

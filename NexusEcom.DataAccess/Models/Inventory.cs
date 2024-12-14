using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.Data.Models
{
    public class Inventory
    {
        public int inventoryName { get; set; }                // Unique Identifier should be same as data source
        public int ProductId { get; set; }         // Product Associated with Inventory
        public int QuantityAvailable { get; set; } // Quantity Available
    }
}

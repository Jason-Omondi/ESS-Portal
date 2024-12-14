using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.Data.Models
{
    public class Product
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }


        public bool IsAvailableInStock(int quantity)
        {
            return StockQuantity >= quantity;
        }

    }

}

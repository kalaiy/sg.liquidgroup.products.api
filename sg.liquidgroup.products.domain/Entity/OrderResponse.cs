using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sg.LiquidGroup.Products.Domain.Entity
{
    public class OrderResponse
    {
        public string OrderId { get; set; }

        public List<Product> Products { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }
    }
}

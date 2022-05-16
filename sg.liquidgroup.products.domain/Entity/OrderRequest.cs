using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sg.LiquidGroup.Products.Domain.Entity
{
    public class OrderRequest
    {
        public string CartId { get; set; }

        public OrderStatus Status { get; set; }
    }
}

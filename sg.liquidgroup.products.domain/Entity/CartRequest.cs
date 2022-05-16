using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sg.LiquidGroup.Products.Domain.Entity
{
    public class CartRequest
    {
        public string Id { get; set; }
        public List<CartProduct> Products { get; set; }
    }
}

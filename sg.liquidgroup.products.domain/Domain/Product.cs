using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sg.LiquidGroup.Products.Domain
{
    public class Product : Document
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public decimal Rating { get; set; }

        public string Category { get; set; } = "";
        
        public decimal Price { get; set; }

        public int Quantity { get; set; }

    }
}

using Sg.LiquidGroup.Products.Domain;
using Sg.LiquidGroup.Products.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sg.liquidgroup.products.api.unittest.Models
{
    public class CartRequestTest
    {
        [Fact]
        public void CartRequestTest_getter_setter()
        {
            var id = "123";
            var products = new List<CartProduct>();
            products.Add(new CartProduct { ProductId="123", Quantity=1 });
            var cardRequest = new CartRequest();
           
            cardRequest.Id = id;
            cardRequest.Products= products;
            Assert.Equal(cardRequest.Id,id);
            Assert.Equal(cardRequest.Products, products);

        }
    }
}

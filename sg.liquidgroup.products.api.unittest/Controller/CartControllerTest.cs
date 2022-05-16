using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sg.LiquidGroup.Products.Api.Controllers;
using Sg.LiquidGroup.Products.Api.Services;
using Xunit;

namespace Sg.LiquidGroup.Products.Api.unittest
{
    public class CartControllerTest
    {
        [Fact]
        public void CartController_GetCart_Return_Ok()
        {
            var logger = new Mock<ILogger<CartController>>();
            var cartService = new Mock<ICartService>();
            CartController controller = new CartController(logger.Object, cartService.Object);
            var actionResult = controller.GetCart().GetAwaiter().GetResult();
            var okResult = actionResult as OkObjectResult;

            Assert.True(okResult.StatusCode.Equals(200));

        }

        [Fact]
        public void CartController_GetCart_Return_Not_Ok()
        {
            var logger = new Mock<ILogger<CartController>>();
            var cartService = new Mock<ICartService>();
            cartService.Setup(x => x.GetCurrentCart(It.IsAny<string>()))
                .Throws(new System.InvalidOperationException());
            CartController controller = new CartController(logger.Object, cartService.Object);
            var actionResult = controller.GetCart().GetAwaiter().GetResult();
            var badRequestResult = actionResult as BadRequestObjectResult;

            Assert.True(badRequestResult.StatusCode.Equals(400));

        }

            
        [Fact]
        public void CartController_AddToCart_Return_Ok()
        {
            var logger = new Mock<ILogger<CartController>>();
            var cartService = new Mock<ICartService>();
            CartController controller = new CartController(logger.Object, cartService.Object);
            var actionResult = controller.AddToCart(new Domain.Entity.CartRequest()).GetAwaiter().GetResult();
            var okResult = actionResult as OkObjectResult;

            Assert.True(okResult.StatusCode.Equals(200));

        }

        [Fact]
        public void CartController_AddToCart_Return_Not_Ok()
        {
            var logger = new Mock<ILogger<CartController>>();
            var cartService = new Mock<ICartService>();
            cartService.Setup(x => x.AddCart(It.IsAny<Domain.Entity.CartRequest>()))
                .Throws(new System.InvalidOperationException());
            CartController controller = new CartController(logger.Object, cartService.Object);
            var actionResult = controller.AddToCart(new Domain.Entity.CartRequest()).GetAwaiter().GetResult();
            var badRequestResult = actionResult as BadRequestObjectResult;

            Assert.True(badRequestResult.StatusCode.Equals(400));

        }

        [Fact]
        public void CartController_DeleteCart_Return_Ok()
        {
            var logger = new Mock<ILogger<CartController>>();
            var cartService = new Mock<ICartService>();
            CartController controller = new CartController(logger.Object, cartService.Object);
            var actionResult = controller.DeleteCart("6280a99a1213e1a82f74d5be").GetAwaiter().GetResult();
            var okResult = actionResult as OkObjectResult;

            Assert.True(okResult.StatusCode.Equals(200));

        }

        [Fact]
        public void CartController_DeleteCart_Return_Not_Ok()
        {
            var logger = new Mock<ILogger<CartController>>();
            var cartService = new Mock<ICartService>();
            cartService.Setup(x => x.DeleteCart(It.IsAny<string>()))
                .Throws(new System.InvalidOperationException());
            CartController controller = new CartController(logger.Object, cartService.Object);
            var actionResult = controller.DeleteCart("6280a99a1213e1a82f74d5be").GetAwaiter().GetResult();
            var badRequestResult = actionResult as BadRequestObjectResult;

            Assert.True(badRequestResult.StatusCode.Equals(400));

        }


        [Fact]
        public void CartController_Checkout_Return_Ok()
        {
            var logger = new Mock<ILogger<CartController>>();
            var cartService = new Mock<ICartService>();
            CartController controller = new CartController(logger.Object, cartService.Object);
            var actionResult = controller.Checkout(new Domain.Order()).GetAwaiter().GetResult();
            var okResult = actionResult as OkObjectResult;

            Assert.True(okResult.StatusCode.Equals(200));

        }

        [Fact]
        public void CartController_Checkout_Return_Not_Ok()
        {
            var logger = new Mock<ILogger<CartController>>();
            var cartService = new Mock<ICartService>();
            cartService.Setup(x => x.CartCheckout(It.IsAny<Domain.Order>()))
                .Throws(new System.InvalidOperationException());
            CartController controller = new CartController(logger.Object, cartService.Object);
            var actionResult = controller.Checkout(new Domain.Order()).GetAwaiter().GetResult();
            var badRequestResult = actionResult as BadRequestObjectResult;

            Assert.True(badRequestResult.StatusCode.Equals(400));

        }
    }
}
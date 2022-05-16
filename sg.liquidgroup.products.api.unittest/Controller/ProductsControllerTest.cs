using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sg.LiquidGroup.Products.Api.Controllers;
using Sg.LiquidGroup.Products.Api.Services;
using Xunit;

namespace Sg.LiquidGroup.Products.Api.unittest
{
    public class ProductsControllerTest
    {
        [Fact]
        public void ProductsController_SearchProduct_Return_Ok()
        {
            var logger = new Mock<ILogger<ProductsController>>();
            var productService = new Mock<IProductService>();
            ProductsController controller = new ProductsController(logger.Object, productService.Object);
            var actionResult = controller.Get("","","").GetAwaiter().GetResult();
            var okResult = actionResult as OkObjectResult;

            Assert.True(okResult.StatusCode.Equals(200));

        }

        [Fact]
        public void ProductsController_SearchProduct_Return_Not_Ok()
        {
            var logger = new Mock<ILogger<ProductsController>>();
            var productService = new Mock<IProductService>();
            productService.Setup(x => x.GetProducts(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new System.InvalidOperationException());
            ProductsController controller = new ProductsController(logger.Object, productService.Object);
            var actionResult = controller.Get("", "", "").GetAwaiter().GetResult();
            var badRequestResult = actionResult as BadRequestObjectResult;

            Assert.True(badRequestResult.StatusCode.Equals(400));

        }
    }
}
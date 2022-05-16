using Microsoft.AspNetCore.Mvc;
using Sg.LiquidGroup.Products.Api.Services;
using Sg.LiquidGroup.Products.Domain;

namespace Sg.LiquidGroup.Products.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/products")]
    public class ProductsController : ControllerBase
    {


        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            this.productService=productService;
        }

        [HttpGet(Name = "list")]
        public async Task<IActionResult> Get([FromQuery] string? query, [FromQuery] string? filter, [FromQuery] string? sortBy)
        {
            List<Product> products;
            try
            {
                 products = await productService.GetProducts(query??"", filter??"", sortBy??"");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(products);
        }


    }
}
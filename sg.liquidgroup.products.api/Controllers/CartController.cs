using Microsoft.AspNetCore.Mvc;

using Sg.LiquidGroup.Products.Api.Services;
using Sg.LiquidGroup.Products.Domain;
using Sg.LiquidGroup.Products.Domain.Entity;

namespace Sg.LiquidGroup.Products.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/cart/[action]")]
    public class CartController : ControllerBase
    {


        private readonly ILogger<CartController> _logger;
        private readonly ICartService cartService;

        public CartController(ILogger<CartController> logger, ICartService cartService)
        {
            _logger = logger;
            this.cartService=cartService;
        }


        [HttpGet, ActionName("all")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetCart()
        {
            Cart cartResponse;
            try
            {
                cartResponse= await cartService.GetCurrentCart("");//UserId HttpContext.User.FindFirst(ClaimTypes.SerialNumber).Value
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }

            return Ok(cartResponse);
        }


        [HttpPost, ActionName("addorupdate")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddToCart([FromBody] CartRequest cart)
        {
            Cart cartResponse;
            try
            {
                
                cartResponse= await cartService.AddCart(cart);
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }

            return Ok(cartResponse);
        }



        [HttpDelete, ActionName("delete")]

        public async Task<IActionResult> DeleteCart([FromQuery] string id)
        {
            try
            {
                await cartService.DeleteCart(id);
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }

            return Ok(true);
        }



        [HttpPost, ActionName("checkout")]
        public async Task<IActionResult> Checkout([FromBody] OrderRequest order)
        {
            OrderResponse? response;
            try
            {
                response = await cartService.CartCheckout(order);
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }

            return Ok(response);
        }
    }
}
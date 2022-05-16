using Sg.LiquidGroup.Products.Domain;
using Sg.LiquidGroup.Products.Domain.Entity;

namespace Sg.LiquidGroup.Products.Api.Services
{
    public interface ICartService
    {
        Task<Cart> AddCart(CartRequest cart);
        Task<OrderResponse?> CartCheckout(OrderRequest orderRequest);
        Task DeleteCart(string cartId);
        Task<Cart> GetCurrentCart(string userId);
        Task<bool> ValidateProductsQuantity(Cart cart);
    }
}
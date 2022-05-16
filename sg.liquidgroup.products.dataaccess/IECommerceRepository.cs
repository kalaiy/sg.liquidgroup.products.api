using Sg.LiquidGroup.Products.Domain;
using Sg.LiquidGroup.Products.Domain.Entity;

namespace Sg.LiquidGroup.Products.Dataaccess
{
    public interface IECommerceRepository
    {
        Task AddMasterData();
        Task<Cart> AddToCart(Cart cartItem);
       
        Task<OrderResponse> CartCheckout(Order order, Cart existingCart);
        Task DeleteCart(string id);
        Task<Cart?> GetCartById(string id, bool throwIfNotExists = true);
        Task<Cart> GetCurrentCart(string userId);
        Task<Product?> GetProductById(string id, bool throwIfNotExists = true);
        Task<List<Product>> GetProducts(string query, string filter, string sortBy);
        Task<List<Product>> GetProductsByCartId(string cartId);
        Task<bool> ValidateProductsQuantity(Cart cart);
    }
}
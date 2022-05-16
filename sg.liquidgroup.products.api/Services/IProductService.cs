using Sg.LiquidGroup.Products.Domain;

namespace Sg.LiquidGroup.Products.Api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts(string query, string filter, string sortBy);
    }
}
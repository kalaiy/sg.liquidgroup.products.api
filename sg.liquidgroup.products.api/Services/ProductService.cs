using Sg.LiquidGroup.Products.Dataaccess;
using Sg.LiquidGroup.Products.Domain;
using System;

namespace Sg.LiquidGroup.Products.Api.Services
{


    public class ProductService : IProductService
    {
        private readonly IECommerceRepository eCommRepository;

        public ProductService(IECommerceRepository eCommRepository,
            IConfiguration appSettings)
        {


            this.eCommRepository=eCommRepository;
        }

        public async Task<List<Product>> GetProducts(string query="", string filter = "", string sortBy = "")
        {

            return await eCommRepository.GetProducts(query, filter, sortBy);
        }
    }

}

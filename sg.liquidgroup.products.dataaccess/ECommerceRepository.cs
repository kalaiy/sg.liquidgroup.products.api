using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sg.LiquidGroup.Products.Domain;
using Sg.LiquidGroup.Products.Domain.Entity;

namespace Sg.LiquidGroup.Products.Dataaccess
{
    public class ECommerceRepository : IECommerceRepository
    {
        private readonly IMongoDatabase database;
        private readonly ILogger<ECommerceRepository> logger;
        private IDictionary<Type, string> dicTableMapper;
        public ECommerceRepository(ILogger<ECommerceRepository> logger, IMongoDatabase database)
        {
            this.database = database;
            this.logger = logger;
            AddMapper();
            //Task.Run(async () => await AddMasterData());
        }


        private void AddMapper()
        {
            dicTableMapper = new Dictionary<Type, string>();
            dicTableMapper.Add(typeof(Product), "products");
            dicTableMapper.Add(typeof(Order), "orders");
            dicTableMapper.Add(typeof(Cart), "cart");

        }

        private string GetCollectionName(Type tableType)
        {
            return dicTableMapper[tableType];
        }

        public async Task AddMasterData()
        {
            try
            {
                IMongoCollection<Product> productCollection = database.GetCollection<Product>(GetCollectionName(typeof(Product)));
                var product = await productCollection.AsQueryable().FirstOrDefaultAsync();
                if (product == null)
                {
                    string contents = await File.ReadAllTextAsync("sampledata.json");
                    if (!string.IsNullOrEmpty(contents))
                    {
                        var products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(contents);
                        await productCollection.InsertManyAsync(products);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AddMasterData");
            }
        }

        public async Task<List<Product>> GetProducts(string query, string filter, string sortBy)
        {

            query= query.ToLower();
            filter= filter.ToLower();
            sortBy= sortBy.ToLower();
            var products = new List<Product>();
            IMongoCollection<Product> productCollection = database.GetCollection<Product>(GetCollectionName(typeof(Product)));
            IQueryable<Product> productQuery = productCollection.AsQueryable();
            decimal decimalQuery = 0;
            var isQueryAsDecimal = Decimal.TryParse(query, out decimalQuery);

            if (isQueryAsDecimal)
            {
                productQuery= productQuery.Where(o => o.Rating==decimalQuery
                || o.Price==decimalQuery);
            }
            else if (!string.IsNullOrEmpty(query))
                productQuery= productQuery.Where(o => o.Name.ToLower().Contains(query)
                || o.Description.ToLower().Contains(query)
                || o.Category.ToLower().Contains(query)
              );


            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.ToLower() == "in-stock")
                    productQuery= productQuery.Where(o => o.Quantity>0);
                if (filter.ToLower() == "out-of-stock")
                    productQuery= productQuery.Where(o => o.Quantity<=0);
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "price-asc":
                        productQuery = productQuery.OrderBy(o => o.Price);
                        break;
                    case "price-desc":
                        productQuery = productQuery.OrderByDescending(o => o.Price);
                        break;
                }
            }
            return await ((IMongoQueryable<Product>)productQuery).ToListAsync();
        }


        public async Task<List<Product>> GetProductsByCartId(string cartId)
        {


            var products = new List<Product>();
            var productCollection = database.GetCollection<Product>(GetCollectionName(typeof(Product)));
            var cartCollection = database.GetCollection<Cart>(GetCollectionName(typeof(Cart)));
            var cart = await GetCartById(cartId, false);
            if (cart != null)
            {

                var productCart = cart.Products.Select(o => o.ProductId);
                products= productCollection.Find(Builders<Product>.Filter.In("Id", productCart))
                      .ToEnumerable().ToList();
            }
            return products;
        }

        public async Task<bool> ValidateProductsQuantity(Cart cart)
        {
            var products = new List<Product>();
            if (!string.IsNullOrEmpty(cart.Id))
            {
                products= await GetProductsByCartId(cart.Id);
            }
            else
            {
                foreach (var product in cart.Products)
                {
                    var productItem = await GetProductById(product.ProductId);
                    if (productItem != null)
                        products.Add(productItem);
                }
            }
            foreach (var product in products)
            {
                var cartItem = cart.Products.FirstOrDefault(o => o.ProductId== product.Id);
                if (product.Quantity<cartItem?.Quantity)
                {
                    return false;
                }

            }
            return true;
        }



        public async Task<Cart> AddToCart(Cart cartItem)
        {
            IMongoCollection<Cart> cartCollection = database.GetCollection<Cart>(GetCollectionName(typeof(Cart)));

            if (string.IsNullOrEmpty(cartItem.Id))
            {
                await cartCollection.InsertOneAsync(cartItem);
            }
            else
            {
                if (await GetCartById(cartItem.Id)!=null)
                {
                    var update = Builders<Cart>.Update.Set("Products", cartItem.Products)
                    .Set("Payment", cartItem.Payment);

                    await cartCollection.UpdateOneAsync(Builders<Cart>.Filter.Eq("Id", cartItem.Id), update);
                }

            }
            return cartItem;

        }


        public async Task DeleteCart(string id)
        {
            IMongoCollection<Cart> cartCollection = database.GetCollection<Cart>(GetCollectionName(typeof(Cart)));

            if (await GetCartById(id)!=null)
            {

                var update = Builders<Cart>.Update.Set("IsDeleted", true);
                await cartCollection.UpdateOneAsync(Builders<Cart>.Filter.Eq("Id", id), update);
            }

        }


        public async Task<OrderResponse> CartCheckout(Order order, Cart existingCart)
        {
            var orderCollection = database.GetCollection<Order>(GetCollectionName(typeof(Order)));
            var cartCollection = database.GetCollection<Cart>(GetCollectionName(typeof(Cart)));
            var productCollection = database.GetCollection<Product>(GetCollectionName(typeof(Product)));

            var response = new OrderResponse();

            if (existingCart!=null)
            {
                await orderCollection.InsertOneAsync(order);
                if (!string.IsNullOrEmpty(order.Id))
                {
                    var update = Builders<Cart>.Update.Set("IsSubmitted", true);
                    await cartCollection.UpdateOneAsync(Builders<Cart>.Filter.Eq("Id", order.CartId), update);

                    foreach (var product in existingCart.Products)
                    {
                        var productUpdate = Builders<Product>.Update.Inc("Quantity", -1 * product.Quantity);
                        await productCollection.UpdateOneAsync(Builders<Product>.Filter.Eq("Id", product.ProductId), productUpdate);
                    }

                    response.OrderId = order.Id;
                    response.Products = await GetProductsByCartId(existingCart.Id);
                    response.TotalPrice= existingCart.Payment.TotalPrice;

                }

            }
            return response;
        }

        public async Task<Cart> GetCurrentCart(string userId)
        {
            IMongoCollection<Cart> cartCollection = database.GetCollection<Cart>(GetCollectionName(typeof(Cart)));

            return await cartCollection.AsQueryable().Where(o => o.IsDeleted==false && o.IsSubmitted==false).FirstOrDefaultAsync();

        }

        public async Task<Cart?> GetCartById(string id, bool throwIfNotExists = true)
        {
            IMongoCollection<Cart> cartCollection = database.GetCollection<Cart>(GetCollectionName(typeof(Cart)));

            var cart = await cartCollection.AsQueryable().FirstOrDefaultAsync(o => o.Id == id);
            if (throwIfNotExists && ((cart!=null && (cart.IsDeleted || cart.IsSubmitted)) || cart==null))
                throw new InvalidOperationException($"Cart not exists for the Id:{id}");
            else
                return cart;

        }

        public async Task<Product?> GetProductById(string id, bool throwIfNotExists = true)
        {
            IMongoCollection<Product> productCollection = database.GetCollection<Product>(GetCollectionName(typeof(Product)));

            var product = await productCollection.AsQueryable().FirstOrDefaultAsync(o => o.Id == id);
            if (throwIfNotExists && product==null)
                throw new InvalidOperationException($"Product not exists for the Id:{id}");
            else
                return product;

        }

    }


}
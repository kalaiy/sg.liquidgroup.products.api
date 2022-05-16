using AutoMapper;
using Sg.LiquidGroup.Products.Dataaccess;
using Sg.LiquidGroup.Products.Domain;
using Sg.LiquidGroup.Products.Domain.Entity;
using System;

namespace Sg.LiquidGroup.Products.Api.Services
{


    public class CartService : ICartService
    {
        private readonly IECommerceRepository eCommRepository;
        private readonly IMapper mapper;

        public CartService(IECommerceRepository eCommRepository, IMapper mapper,
            IConfiguration appSettings)
        {


            this.eCommRepository=eCommRepository;
            this.mapper=mapper;
        }


        public async Task<Cart> AddCart(CartRequest cart)
        {

            var cartItem = mapper.Map<Cart>(cart);
            await Validate(cartItem);
            await UpdatePayment(cartItem);
            return await eCommRepository.AddToCart(cartItem);
        }



        public async Task DeleteCart(string cartId)
        {
            await eCommRepository.DeleteCart(cartId);
        }

        public async Task<OrderResponse?> CartCheckout(OrderRequest orderRequest)
        {
            var order = mapper.Map<Order>(orderRequest);
            var existingCart = await eCommRepository.GetCartById(order.CartId);
            if (existingCart!= null)
            {
                await Validate(existingCart);
               return await eCommRepository.CartCheckout(order, existingCart);
            }
            return null;
        }

        public async Task<Cart> GetCurrentCart(string userId)
        {
            return await eCommRepository.GetCurrentCart(userId);
        }

        public async Task<bool> ValidateProductsQuantity(Cart cart)
        {
            return await eCommRepository.ValidateProductsQuantity(cart);
        }

        private async Task UpdatePayment(Cart cart)
        {
            List<Product> products = new List<Product>();
            if (!string.IsNullOrEmpty(cart.Id))
            {
                products = await eCommRepository.GetProductsByCartId(cart.Id);

            }
            else
            {
                foreach (var product in cart.Products)
                {
                    var productItem = await eCommRepository.GetProductById(product.ProductId);
                    if (productItem != null)
                        products.Add(productItem);
                }

            }
            var totalPrice = 0m;
            foreach (var product in products)
            {
                totalPrice+=product.Price;
            }
            cart.Payment = new Payment() { Currency="SGD", TotalPrice=totalPrice };
        }

        private async Task Validate(Cart cart)
        {
            if (!await ValidateProductsQuantity(cart))
                throw new InvalidOperationException("Insufficent product quantity!");
        }

        
    }

}

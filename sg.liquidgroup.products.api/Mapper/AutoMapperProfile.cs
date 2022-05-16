using AutoMapper;
using Sg.LiquidGroup.Products.Domain;
using Sg.LiquidGroup.Products.Domain.Entity;

namespace Sg.LiquidGroup.Products.Api.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CartRequest, Cart>();

            CreateMap<OrderRequest, Order>();

        }
    }
}

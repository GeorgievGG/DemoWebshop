using AutoMapper;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.DTOs.ResponseModels;

namespace DemoWebshopApi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            SetupProductMaps();
            SetupOrderMaps();
        }

        private void SetupProductMaps()
        {
            CreateMap<ProductRequestDto, Product>();
            CreateMap<Product, ProductResponseDto>();
        }

        private void SetupOrderMaps()
        {
            CreateMap<OrderRequestDto, Order>();
            CreateMap<OrderLineRequestDto, OrderLine>();
            CreateMap<Order, OrderResponseDto>();
            CreateMap<OrderLine, OrderLineResponseDto>();
        }
    }
}

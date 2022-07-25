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
            SetupUserMaps();
            SetupProductMaps();
            SetupOrderMaps();
            SetupSoppingBasketMaps();
        }

        private void SetupUserMaps()
        {
            CreateMap<CreateUserRequestDto, User>();
            CreateMap<UpdateUserRequestDto, User>();
            CreateMap<User, UserResponseDto>();
            CreateMap<User, UserSensitiveResponseDto>();
        }

        private void SetupProductMaps()
        {
            CreateMap<ProductRequestDto, Product>();
            CreateMap<Product, ProductBaseResponseDto>();
            CreateMap<Product, ProductResponseDto>();
        }

        private void SetupOrderMaps()
        {
            CreateMap<OrderRequestDto, Order>();
            CreateMap<OrderLineRequestDto, OrderLine>();
            CreateMap<Order, OrderResponseDto>();
            CreateMap<OrderLine, OrderLineResponseDto>();
        }

        private void SetupSoppingBasketMaps()
        {
            CreateMap<ShoppingBasketLineRequestDto, ShoppingBasketLine>();
            CreateMap<ShoppingBasket, ShoppingBasketResponseDto>();
            CreateMap<ShoppingBasket, ShoppingBasketDetailedResponseDto>();
            CreateMap<ShoppingBasketLine, ShoppingBasketLineResponseDto>();
            CreateMap<ShoppingBasketLine, ShoppingBasketDetailedLineResponseDto>();
        }
    }
}

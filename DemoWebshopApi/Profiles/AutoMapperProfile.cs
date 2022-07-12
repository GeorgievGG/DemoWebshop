using AutoMapper;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.DTO.RequestModels;
using DemoWebshopApi.DTO.ResponseModels;

namespace DemoWebshopApi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            SetupProductMaps();
        }

        private void SetupProductMaps()
        {
            CreateMap<ProductRequestDto, Product>();
            CreateMap<Product, ProductResponseDto>();
        }
    }
}

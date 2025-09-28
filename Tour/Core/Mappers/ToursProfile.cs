using API.Dtos;
using API.Dtos.Shopping;
using AutoMapper;
using Core.Domain;
using Core.Domain.Shopping;

namespace Core.Mappers
{
    public class ToursProfile : Profile
    {
        public ToursProfile()
        {
            CreateMap<Tour, TourDto>().ReverseMap();
            CreateMap<Tag, TagDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Checkpoint, CheckpointDto>().ReverseMap();
            CreateMap<ShoppingCartItemCreationDto, ShoppingCartItem>();
            CreateMap<ShoppingCartItem, ShoppingCartItemDto>();
        }
    }
}

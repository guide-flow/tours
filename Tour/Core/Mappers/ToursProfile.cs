using API.Dtos;
using AutoMapper;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CreateMap<TransportDuration, TransportDurationDto>().ReverseMap();  
        }
    }
}

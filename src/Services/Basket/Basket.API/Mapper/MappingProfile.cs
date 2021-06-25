using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Discount.Grpc.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}

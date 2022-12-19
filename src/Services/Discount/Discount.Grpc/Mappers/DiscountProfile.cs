using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mappers;

public class DiscountProfile : Profile
{
  protected DiscountProfile()
  {
    CreateMap<Coupon, CouponModel>().ReverseMap();
  }
}
using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

/// <summary>
/// This class is like the Controller in a RESTful API
/// </summary>
public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
  private readonly IDiscountRepository _repository;
  private readonly IMapper _mapper;
  private readonly ILogger<DiscountService> _logger;

  public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
  {
    _repository = repository;
    _mapper = mapper;
    _logger = logger;
  }

  public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
  {
    var coupon = await _repository.GetDiscountAsync(request.ProductName);

    if (coupon == null)
    {
      throw new RpcException(new Status(StatusCode.NotFound,
        $"Discount with ProductName={request.ProductName} is not found."));
    }

    _logger.LogInformation("Discount retrieved successfully.");

    // map to gRPC CouponModel we will be returning
    return _mapper.Map<CouponModel>(coupon);
  }

  public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
  {
    var coupon = _mapper.Map<Coupon>(request.Coupon);

    await _repository.CreateDiscountAsync(coupon);

    // we will be returning the id information as well
    return _mapper.Map<CouponModel>(coupon);
  }

  public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
  {
    var coupon = _mapper.Map<Coupon>(request.Coupon);

    await _repository.UpdateDiscountAsync(coupon);

    // we will be returning the id information as well
    return _mapper.Map<CouponModel>(coupon);
  }

  public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
    ServerCallContext context)
  {
    var success = await _repository.DeleteDiscountAsync(request.ProductName);
    return new DeleteDiscountResponse { Success = success };
  }
}
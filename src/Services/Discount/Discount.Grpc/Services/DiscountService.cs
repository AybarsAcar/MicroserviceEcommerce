using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
  private readonly IDiscountRepository _repository;
  private readonly ILogger<DiscountService> _logger;

  public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger)
  {
    _repository = repository;
    _logger = logger;
  }

  public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
  {
    var coupon = await _repository.GetDiscountAsync(request.ProductName);

    if (coupon == null)
    {
      throw new RpcException(new Status(StatusCode.NotFound,
        $"Discount with ProductName={request.ProductName} is not found."))
    }

    // map to gRPC CouponModel we will be returning
    return _mapper.Map<CouponModel>(coupon);
  }
}
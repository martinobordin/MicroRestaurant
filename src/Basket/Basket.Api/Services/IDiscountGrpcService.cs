using Discount.Grpc.Protos;

namespace Basket.Api.Services;

public interface IDiscountGrpcService
{
    Task<CouponModel> GetDiscountAsync(string productName, CancellationToken token = default);
}
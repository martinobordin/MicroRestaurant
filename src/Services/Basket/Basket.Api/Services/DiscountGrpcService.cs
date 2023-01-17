using Discount.Grpc.Protos;

namespace Basket.Api.Services;

public class DiscountGrpcService : IDiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        this.discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<CouponModel> GetDiscountAsync(string productName, CancellationToken token = default)
    {
        var getDiscountRequest = new GetDiscountRequest { ProductName = productName };
        return await this.discountProtoServiceClient.GetDiscountAsync(getDiscountRequest,  cancellationToken : token);
    }
}

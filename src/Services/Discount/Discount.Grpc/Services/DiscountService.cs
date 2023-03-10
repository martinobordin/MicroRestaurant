using AutoMapper;
using Discount.Grpc.Data;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository discountRepository;
    private readonly IMapper mapper;
    private readonly ILogger<DiscountService> logger;

    public DiscountService(IDiscountRepository discountRepository, IMapper mapper, ILogger<DiscountService> logger)
    {
        this.discountRepository = discountRepository;
        this.mapper = mapper;
        this.logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await discountRepository.GetDiscountAsync(request.ProductName);
        if (coupon == null)
        {
            coupon = new Coupon() 
            { 
                Id = 0,
                ProductName= request.ProductName,
                Amount = 0,
                Description = "NoDiscount"
            };

            //throw new RpcException(new Status(StatusCode.NotFound, $"Discount for product {request.ProductName} not found."));
        }
        logger.LogInformation("Discount is retrieved for ProductName {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = mapper.Map<Coupon>(request.Coupon);

        await discountRepository.CreateDiscountAsync(coupon);
        logger.LogInformation("Discount is successfully created for ProductName {ProductName}", coupon.ProductName);

        var couponModel = mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = mapper.Map<Coupon>(request.Coupon);

        await discountRepository.UpdateDiscountAsync(coupon);
        logger.LogInformation("Discount is successfully updated for ProductName {ProductName}", coupon.ProductName);

        var couponModel = mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await discountRepository.DeleteDiscountAsync(request.ProductName);
        var response = new DeleteDiscountResponse
        {
            Success = deleted
        };

        return response;
    }
}

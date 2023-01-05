using Discount.Api.Entities;

namespace Discount.Api.Data;

public interface IDiscountRepository
{
    Task<Coupon?> GetDiscountAsync(string productName);
    Task<Coupon> CreateDiscountAsync(Coupon coupon);
    Task<Coupon> UpdateDiscountAsync(Coupon coupon);
    Task<bool> DeleteDiscountAsync(string productName);
}
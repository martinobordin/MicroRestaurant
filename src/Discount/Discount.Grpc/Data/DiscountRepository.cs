using Dapper;
using Discount.Grpc.Entities;
using Npgsql;
using System.Data;

namespace Discount.Grpc.Data;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<Coupon?> GetDiscountAsync(string productName)
    {
        using var connection = GetConnection();

        return await connection.QueryFirstOrDefaultAsync<Coupon?>(
                                                    $@"SELECT 
                                                            Id as {nameof(Coupon.Id)},
                                                            ProductName as {nameof(Coupon.ProductName)},
                                                            Description as {nameof(Coupon.Description)},
                                                            Amount as {nameof(Coupon.Amount)}
                                                        FROM Coupon 
                                                        WHERE ProductName = @ProductName",
                                                    new { ProductName = productName });
    }

    public async Task<Coupon> CreateDiscountAsync(Coupon coupon)
    {
        using var connection = GetConnection();

        await connection.ExecuteAsync(
                    "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                    new { coupon.ProductName, coupon.Description, coupon.Amount });

        return coupon;

    }

    public async Task<Coupon> UpdateDiscountAsync(Coupon coupon)
    {
        using var connection = GetConnection();

        await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                                   new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

        return coupon;
    }

    public async Task<bool> DeleteDiscountAsync(string productName)
    {
        using var connection = GetConnection();

        var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
           new { ProductName = productName });

        return affected != 0;
    }

    private IDbConnection GetConnection()
    {
        return new NpgsqlConnection(configuration.GetConnectionString("PostgreSQL"));
    }
}

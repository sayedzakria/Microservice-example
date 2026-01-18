using static Discount.Grpc.DiscountService;

namespace Discount.Grpc.Services
{
    public class DiscountService (DiscountContext _context, ILogger<DiscountService> logger)
        : DiscountServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.ProductName))
                {
                    logger.LogWarning("GetDiscount called with null or empty ProductName");
                    var defaultCoupon = new Coupon
                    {
                        ProductName = "No Discount",
                        Description = "No Discount Description",
                        Amount = 0
                    };
                    return defaultCoupon.Adapt<CouponModel>();
                }

                logger.LogInformation("Processing GetDiscount request for ProductName: {ProductName}", request.ProductName);
                
                var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
                
                if (coupon == null)
                {
                    logger.LogInformation("No discount found for ProductName: {ProductName}, returning default coupon", request.ProductName);
                    coupon = new Coupon
                    {
                        ProductName = "No Discount",
                        Description = "No Discount Description",
                        Amount = 0
                    };
                }
                else
                {
                    logger.LogInformation("Discount retrieved for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
                }
                
                var couponModel = coupon.Adapt<CouponModel>();
                return couponModel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing GetDiscount request for ProductName: {ProductName}. Connection string: {ConnectionString}", 
                    request.ProductName, _context.Database.GetConnectionString());
                throw;
            }
        }
        public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            return base.CreateDiscount(request, context);
        }
        public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            return base.UpdateDiscount(request, context);
        }
        public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            return base.DeleteDiscount(request, context);
        }
    }
}

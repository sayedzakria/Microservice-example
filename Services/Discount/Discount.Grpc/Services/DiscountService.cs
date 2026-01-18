using static Discount.Grpc.DiscountService;

namespace Discount.Grpc.Services
{
    public class DiscountService (DiscountContext _context, ILogger<DiscountService> logger)
        : DiscountServiceBase
    {
        public override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
          var coupon = _context.Coupons.FirstOrDefault(c => c.ProductName.Trim().ToLower() == request.ProductName.Trim().ToLower());
            if (coupon == null)
           coupon= new Coupon
            {
                ProductName = "No Discount",
                Description = "No Discount Description",
                Amount = 0
            };
            logger.LogInformation("Request ProductName: {ProductName}", request.ProductName);
var allCoupons = _context.Coupons.ToList();
logger.LogInformation("Coupons in database: {@Coupons}", allCoupons);
            logger.LogInformation("Discount retrieved for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
            var couponModel = coupon.Adapt<CouponModel>();
            return Task.FromResult(couponModel);
        }
        public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
           var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon data is required"));
            }
            _context.Coupons.Add(coupon);
            _context.SaveChanges();
            logger.LogInformation("Discount created for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
            var couponModel = coupon.Adapt<CouponModel>();
            return Task.FromResult(couponModel);
        }
        public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            var existingCoupon = _context.Coupons.FirstOrDefault(c => c.Id == coupon.Id);
            if (existingCoupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with Id={coupon.Id} not found"));
            }
            existingCoupon.ProductName = coupon.ProductName;
            existingCoupon.Description = coupon.Description;
            existingCoupon.Amount = coupon.Amount;
            _context.SaveChanges();
            logger.LogInformation("Discount updated for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
            var couponModel = existingCoupon.Adapt<CouponModel>();
            return Task.FromResult(couponModel);
        }
        public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.ProductName.Trim().ToLower() == request.ProductName.Trim().ToLower());
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with ProductName={request.ProductName} not found"));
            }
            _context.Coupons.Remove(coupon);
            _context.SaveChanges();
            logger.LogInformation("Discount deleted for ProductName: {ProductName}", request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = true
            };
            return Task.FromResult(response);
        }
    }
}

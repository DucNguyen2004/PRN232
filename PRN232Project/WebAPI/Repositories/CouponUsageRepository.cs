using BusinessObjects;

namespace Repositories
{
    public class CouponUsageRepository : ICouponUsageRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponUsageRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddUsageAsync(int couponId, int userId)
        {
            if (couponId <= 0) throw new ArgumentException("Invalid coupon ID.", nameof(couponId));
            if (userId <= 0) throw new ArgumentException("Invalid user ID.", nameof(userId));
            var usage = new CouponUsage
            {
                CouponId = couponId,
                UserId = userId,
                UsedAt = DateTime.UtcNow
            };
            await _context.CouponUsages.AddAsync(usage);
            await _context.SaveChangesAsync();
        }

    }
}

using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _context.Coupons
                .Include(c => c.Product)
                .Include(c => c.Category)
                .Include(c => c.Usages)
                .ToListAsync();
        }

        public async Task<Coupon> GetByCodeAsync(string code)
        {
            return await _context.Coupons
                .Include(c => c.Product)
                .Include(c => c.Category)
                .Include(c => c.Usages)
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<Coupon> GetByIdAsync(int id)
        {
            return await _context.Coupons
                .Include(c => c.Product)
                .Include(c => c.Category)
                .Include(c => c.Usages)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Coupon> AddAsync(Coupon coupon)
        {
            if (coupon == null) throw new ArgumentNullException(nameof(coupon));

            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(coupon.Id);
        }

        public async Task UpdateAsync(int id, Coupon coupon)
        {
            if (coupon == null) throw new ArgumentNullException(nameof(coupon));

            var tracked = await _context.Coupons.FindAsync(id);
            if (tracked == null)
                throw new KeyNotFoundException($"Coupon with Id {id} not found.");

            coupon.Id = id;
            _context.Entry(tracked).CurrentValues.SetValues(coupon);

            await _context.SaveChangesAsync();
        }
    }
}

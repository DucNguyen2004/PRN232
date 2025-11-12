using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItems(int userId) // change parameter ?
        {
            return await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                    .ThenInclude(p => p.ProductImages)
                .Include(c => c.ProductOptions)
                    .ThenInclude(po => po.OptionValue)
                        .ThenInclude(ov => ov.Option)
                .ToListAsync();
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CartItem> AddToCartAsync(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();

            return await _context.CartItems
                                .Include(ci => ci.User)
                                .Include(ci => ci.Product)
                                    .ThenInclude(p => p.ProductImages)
                                .Include(ci => ci.ProductOptions)
                                    .ThenInclude(po => po.OptionValue)
                                        .ThenInclude(ov => ov.Option)
                                .FirstAsync(ci => ci.Id == item.Id);
        }

        public async Task UpdateAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.CartItems
                .Include(ci => ci.ProductOptions)
                .FirstOrDefaultAsync(ci => ci.Id == id);

            if (item != null)
            {
                item.ProductOptions.Clear();
                await _context.SaveChangesAsync();

                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var items = await _context.CartItems
                .Include(ci => ci.ProductOptions)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            foreach (var item in items)
            {
                item.ProductOptions.Clear();
            }
            await _context.SaveChangesAsync();

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int userId, int productId, List<int> productOptionIds)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.ProductOptions)
                .Where(ci => ci.UserId == userId && ci.ProductId == productId)
                .ToListAsync();

            foreach (var item in cartItems)
            {
                var existingOptionIds = item.ProductOptions.Select(po => po.Id).OrderBy(id => id).ToList();
                if (existingOptionIds.SequenceEqual(productOptionIds.OrderBy(id => id)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

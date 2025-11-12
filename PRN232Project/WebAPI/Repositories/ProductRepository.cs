using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductOptions)
                    .ThenInclude(po => po.OptionValue)
                        .ThenInclude(ov => ov.Option)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductOptions)
                    .ThenInclude(po => po.OptionValue)
                        .ThenInclude(ov => ov.Option)
                .ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(product.Id);
        }

        public async Task UpdateAsync(int id, Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            var tracked = await _context.Products.FindAsync(id);
            if (tracked == null)
                throw new KeyNotFoundException($"Product with Id {id} not found.");

            product.Id = tracked.Id;
            _context.Entry(tracked).CurrentValues.SetValues(product);

            tracked.ProductOptions.Clear();
            foreach (var option in product.ProductOptions)
            {
                tracked.ProductOptions.Add(option);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with Id {id} not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}

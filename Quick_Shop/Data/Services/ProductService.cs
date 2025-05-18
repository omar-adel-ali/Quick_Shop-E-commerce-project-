using Microsoft.EntityFrameworkCore;
using Quick_Shop.Data;
using Quick_Shop.Data.Base;
using Quick_Shop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public class ProductsService : EntityBaseRepository<Product>, IProductsService
    {
        private readonly AppDbContext _context;

        public ProductsService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchString, string category = null, decimal? maxPrice = null)
        {
            var products = await GetAllAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower()) ||
                                              p.Category.ToLower().Contains(searchString.ToLower()));
            }
            if (!string.IsNullOrEmpty(category))
            {
                var categories = category.Split(',');
                products = products.Where(p => categories.Contains(p.Category.ToLower()));
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetFeaturedAsync()
        {
            // [Commented out for future use]
            // return await _context.Products
            //     .Where(p => p.IsFeatured)
            //     .Take(6)
            //     .ToListAsync();

            // Replacement: Return first 6 products as a fallback
            return await _context.Products
                .Take(6)
                .ToListAsync();
        }
    }
}
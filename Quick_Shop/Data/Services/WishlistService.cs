using Microsoft.EntityFrameworkCore;
using Quick_Shop.Data;
using Quick_Shop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public class WishlistsService : IWishlistsService
    {
        private readonly AppDbContext _context;

        public WishlistsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddToWishlistAsync(Wishlist wishlistItem)
        {
            var existingItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == wishlistItem.UserId && w.ProductId == wishlistItem.ProductId);
            if (existingItem == null)
            {
                await _context.Wishlists.AddAsync(wishlistItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Wishlist>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var wishlistItem = await _context.Wishlists.FindAsync(id);
            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Wishlist> GetByIdAsync(int id)
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
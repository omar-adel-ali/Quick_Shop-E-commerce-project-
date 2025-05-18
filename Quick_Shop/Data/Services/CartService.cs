using Microsoft.EntityFrameworkCore;
using Quick_Shop.Data;
using Quick_Shop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public class CartsService : ICartsService
    {
        private readonly AppDbContext _context;

        public CartsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(Cart cartItem)
        {
            var existingItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == cartItem.UserId && c.ProductId == cartItem.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
                _context.Carts.Update(existingItem);
            }
            else
            {
                await _context.Carts.AddAsync(cartItem);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Cart>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cartItem = await _context.Carts.FindAsync(id);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateQuantityAsync(int id, int quantity)
        {
            var cartItem = await _context.Carts.FindAsync(id);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                if (cartItem.Quantity <= 0)
                {
                    _context.Carts.Remove(cartItem);
                }
                else
                {
                    _context.Carts.Update(cartItem);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetByIdAsync(int id)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
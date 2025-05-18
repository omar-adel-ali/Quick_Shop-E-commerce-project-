using Quick_Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public interface IWishlistsService
    {
        Task AddToWishlistAsync(Wishlist wishlistItem);
        Task<List<Wishlist>> GetAllByUserIdAsync(string userId);
        Task DeleteAsync(int id);
        Task<Wishlist> GetByIdAsync(int id);
    }
}
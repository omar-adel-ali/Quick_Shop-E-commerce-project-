using Quick_Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public interface ICartsService
    {
        Task AddToCartAsync(Cart cartItem);
        Task<List<Cart>> GetAllByUserIdAsync(string userId);
        Task DeleteAsync(int id);
        Task UpdateQuantityAsync(int id, int quantity);
        Task ClearCartAsync(string userId);
        Task<Cart> GetByIdAsync(int id);
    }
}
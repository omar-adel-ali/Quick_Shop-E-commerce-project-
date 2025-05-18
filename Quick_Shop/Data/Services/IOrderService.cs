using Quick_Shop.Data.ViewModels;
using Quick_Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public interface IOrdersService
    {
        Task<List<Order>> GetOrdersByUserIdAsync(string userId, string userRole);
        Task StoreOrderAsync(IEnumerable<Cart> items, string userId, string userEmailAddress, CheckoutVM checkout);
        Task UpdateOrderStatusAsync(int id, string status);
        Task<List<Order>> GetAllOrdersAsync();
    }
}
using Quick_Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public interface IOrderItemService
    {
        Task AddAsync(OrderItem orderItem);
        Task<List<OrderItem>> GetAllByOrderIdAsync(int orderId);
        Task DeleteAsync(int id);
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quick_Shop.Data;
using Quick_Shop.Data.ViewModels;
using Quick_Shop.Models;
using Quick_Shop.Models.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public OrdersService(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId, string userRole)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            if (userRole != "Admin")
            {
                orders = orders.Where(o => o.UserId == userId).ToList();
            }

            return orders;
        }

        public async Task StoreOrderAsync(IEnumerable<Cart> items, string userId, string userEmailAddress, CheckoutVM checkout)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var order = new Order
            {
                UserId = userId,
                Email = userEmailAddress,
                OrderDate = DateTime.Now,
                Subtotal = items.Sum(c => c.Quantity * c.Product.Price),
                Shipping = 10,
                Total = items.Sum(c => c.Quantity * c.Product.Price) + 10,
                FullName = checkout.FullName,
                City = checkout.City,
                Country = checkout.Country,
                ZipCode = checkout.ZipCode,
                OrderStatus = "Pending",
                OrderItems = items.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Product.Price
                }).ToList()
            };

            // [Commented out for future use]
            // foreach (var item in items)
            // {
            //     var product = await _context.Products.FindAsync(item.ProductId);
            //     if (product.StockQuantity < item.Quantity)
            //     {
            //         throw new InvalidOperationException($"Insufficient stock for product {product.Name}.");
            //     }
            //     product.StockQuantity -= item.Quantity;
            // }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusAsync(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.OrderStatus = status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }
    }
}
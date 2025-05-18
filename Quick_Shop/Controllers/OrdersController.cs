using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quick_Shop.Data.Services;
using Quick_Shop.Data.ViewModels;
using Quick_Shop.Models;
using Quick_Shop.Models.Models;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quick_Shop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly ICartsService _cartsService;
        private readonly UserManager<Users> _userManager;
        private readonly PaymentService _paymentService;

        public OrdersController(IOrdersService ordersService, ICartsService cartsService, UserManager<Users> userManager, PaymentService paymentService)
        {
            _ordersService = ordersService;
            _cartsService = cartsService;
            _userManager = userManager;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? "Customer";
            var orders = await _ordersService.GetOrdersByUserIdAsync(userId, userRole);
            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageOrders()
        {
            var orders = await _ordersService.GetAllOrdersAsync();
            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _ordersService.GetAllOrdersAsync();
            var selectedOrder = order.FirstOrDefault(o => o.Id == id);
            if (selectedOrder == null)
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction(nameof(ManageOrders));
            }
            return View(selectedOrder);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _ordersService.GetAllOrdersAsync();
            if (!order.Any(o => o.Id == id))
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction(nameof(ManageOrders));
            }
            await _ordersService.UpdateOrderStatusAsync(id, status);
            TempData["Success"] = "Order status updated successfully!";
            return RedirectToAction(nameof(ManageOrders));
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _cartsService.GetAllByUserIdAsync(userId);
            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty. Please add items to proceed.";
                return RedirectToAction("Index", "Cart");
            }

            var checkoutVM = new CheckoutVM
            {
                Email = User.FindFirstValue(ClaimTypes.Email),
                FullName = (await _userManager.FindByIdAsync(userId))?.FullName,
                CartItems = cartItems,
                Total = cartItems.Sum(c => c.Quantity * c.Product.Price) + 10 // رسوم شحن
            };
            return View(checkoutVM);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(CheckoutVM checkoutVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields correctly.";
                checkoutVM.CartItems = await _cartsService.GetAllByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View("Checkout", checkoutVM);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var cartItems = await _cartsService.GetAllByUserIdAsync(userId);

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty. Please add items to proceed.";
                return RedirectToAction("Index", "Cart");
            }

            // [Commented out for future use]
            // foreach (var item in cartItems)
            // {
            //     if (item.Product.StockQuantity < item.Quantity)
            //     {
            //         TempData["Error"] = $"Insufficient stock for '{item.Product.Name}'. Available: {item.Product.StockQuantity}.";
            //         return RedirectToAction("Index", "Cart");
            //     }
            // }

            try
            {
                var total = cartItems.Sum(c => c.Quantity * c.Product.Price) + 10;
                var orderId = await _paymentService.CreateOrderAsync(total);

                TempData["CheckoutVM"] = JsonSerializer.Serialize(checkoutVM);
                return Redirect($"https://www.sandbox.paypal.com/checkoutnow?token={orderId}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to initiate payment. Please try again or contact support.";
                return RedirectToAction("Checkout");
            }
        }

        public async Task<IActionResult> CaptureOrder(string token)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var cartItems = await _cartsService.GetAllByUserIdAsync(userId);

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            try
            {
                var success = await _paymentService.CaptureOrderAsync(token);
                if (success)
                {
                    var checkoutVM = JsonSerializer.Deserialize<CheckoutVM>((string)TempData["CheckoutVM"]);
                    await _ordersService.StoreOrderAsync(cartItems, userId, userEmail, checkoutVM);
                    await _cartsService.ClearCartAsync(userId);
                    TempData["Success"] = "Order placed successfully!";
                    return View("OrderCompleted");
                }

                TempData["Error"] = "Payment failed. Please try again.";
                return RedirectToAction("Checkout");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred during payment processing. Please try again or contact support.";
                return RedirectToAction("Checkout");
            }
        }
    }
}
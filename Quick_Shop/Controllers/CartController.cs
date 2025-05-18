using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quick_Shop.Data.Services;
using Quick_Shop.Data.ViewModels;
using Quick_Shop.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Quick_Shop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICartsService _cartsService;

        public CartController(IProductsService productsService, ICartsService cartsService)
        {
            _productsService = productsService;
            _cartsService = cartsService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _cartsService.GetAllByUserIdAsync(userId);

            var response = new ShoppingCartVM
            {
                CartItems = cartItems,
                CartTotal = cartItems.Sum(c => c.Quantity * c.Product.Price)
            };

            return View(response);
        }

        public async Task<IActionResult> AddItemToCart(int id)
        {
            var product = await _productsService.GetByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Product not found. Please try another product.";
                return RedirectToAction("Index", "Product");
            }
            // [Commented out for future use]
            // if (product.StockQuantity < 1)
            // {
            //     TempData["Error"] = $"Product '{product.Name}' is out of stock.";
            //     return RedirectToAction("Index", "Product");
            // }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = new Cart
            {
                UserId = userId,
                ProductId = id,
                Quantity = 1
            };
            await _cartsService.AddToCartAsync(cartItem);
            TempData["Success"] = "Product added to cart successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCartAjax(int id)
        {
            var product = await _productsService.GetByIdAsync(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }
            // [Commented out for future use]
            // if (product.StockQuantity < 1)
            // {
            //     return Json(new { success = false, message = $"Product '{product.Name}' is out of stock." });
            // }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = new Cart
            {
                UserId = userId,
                ProductId = id,
                Quantity = 1
            };
            await _cartsService.AddToCartAsync(cartItem);
            return Json(new { success = true, message = "Product added to cart successfully!" });
        }

        public async Task<IActionResult> RemoveItemFromCart(int id)
        {
            var cartItem = await _cartsService.GetByIdAsync(id);
            if (cartItem == null)
            {
                TempData["Error"] = "Cart item not found.";
                return RedirectToAction(nameof(Index));
            }
            await _cartsService.DeleteAsync(id);
            TempData["Success"] = "Item removed from cart successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            if (quantity < 1)
            {
                TempData["Error"] = "Quantity must be at least 1.";
                return RedirectToAction(nameof(Index));
            }
            var cartItem = await _cartsService.GetByIdAsync(id);
            if (cartItem == null)
            {
                TempData["Error"] = "Cart item not found.";
                return RedirectToAction(nameof(Index));
            }
            // [Commented out for future use]
            // if (cartItem.Product.StockQuantity < quantity)
            // {
            //     TempData["Error"] = $"Requested quantity ({quantity}) exceeds available stock ({cartItem.Product.StockQuantity}) for '{cartItem.Product.Name}'.";
            //     return RedirectToAction(nameof(Index));
            // }
            await _cartsService.UpdateQuantityAsync(id, quantity);
            TempData["Success"] = "Cart updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart(string returnUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _cartsService.GetAllByUserIdAsync(userId);

            if (cartItems.Any())
            {
                foreach (var item in cartItems)
                {
                    await _cartsService.DeleteAsync(item.Id);
                }
                TempData["Success"] = "Cart cleared successfully!";
            }
            else
            {
                TempData["Info"] = "Your cart is already empty.";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
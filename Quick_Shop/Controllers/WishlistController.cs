using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quick_Shop.Data.Services;
using Quick_Shop.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Quick_Shop.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly IWishlistsService _wishlistsService;

        public WishlistController(IProductsService productsService, IWishlistsService wishlistsService)
        {
            _productsService = productsService;
            _wishlistsService = wishlistsService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistItems = await _wishlistsService.GetAllByUserIdAsync(userId);
            return View(wishlistItems);
        }

        public async Task<IActionResult> AddToWishlist(int id)
        {
            var product = await _productsService.GetByIdAsync(id);
            if (product != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishlistItem = new Wishlist
                {
                    UserId = userId,
                    ProductId = id
                };
                await _wishlistsService.AddToWishlistAsync(wishlistItem);
                TempData["Success"] = "Product added to wishlist!";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            await _wishlistsService.DeleteAsync(id);
            TempData["Success"] = "Product removed from wishlist!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearWishlist(string returnUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistItems = await _wishlistsService.GetAllByUserIdAsync(userId);

            if (wishlistItems.Any())
            {
                foreach (var item in wishlistItems)
                {
                    await _wishlistsService.DeleteAsync(item.Id);
                }
                TempData["Success"] = "Wishlist cleared successfully!";
            }
            else
            {
                TempData["Info"] = "Your wishlist is already empty.";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
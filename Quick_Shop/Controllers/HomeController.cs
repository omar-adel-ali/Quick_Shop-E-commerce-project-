using Microsoft.AspNetCore.Mvc;
using Quick_Shop.Data.Services;
using Quick_Shop.Data.ViewModels;
using Quick_Shop.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Quick_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService _productsService;

        public HomeController(ILogger<HomeController> logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = await _productsService.GetFeaturedAsync();

            var model = new ProductSearchVM
            {
                Products = featuredProducts.ToList()
                // ممكن تضيف خصائص بحث هنا إذا تريد
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

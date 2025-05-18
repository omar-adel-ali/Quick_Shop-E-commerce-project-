using Microsoft.AspNetCore.Mvc;
using Quick_Shop.Data;
using Quick_Shop.Data.ViewModels;
using Quick_Shop.Models;
using System.Linq;
using System.Collections.Generic;

namespace Quick_Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(ProductSearchVM model)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(model.SearchString))
            {
                products = products.Where(p => p.Name.Contains(model.SearchString) || p.Description.Contains(model.SearchString));
            }

            if (model.Category != null && model.Category.Any())
            {
                products = products.Where(p => model.Category.Contains(p.Category));
            }

            if (model.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= model.MaxPrice.Value);
            }

            model.Products = products.ToList();
            // التأكد من أن كل منتج ليه صورة
            foreach (var product in model.Products)
            {
                if (string.IsNullOrEmpty(product.Image))
                {
                    product.Image = "/images/fallback-image.jpg";
                }
            }

            ViewBag.SortBy = Request.Query["sortBy"];
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            // التأكد من أن Image مش null أو فارغ
            if (string.IsNullOrEmpty(product.Image))
            {
                product.Image = "/images/fallback-image.jpg";
            }
            // التأكد من أن Images مش null (اختياري لو بتستخدمه)
            if (string.IsNullOrEmpty(product.Images))
            {
                product.Images = "[]"; // قائمة JSON فاضية
            }
            return View(product);
        }
    }
}
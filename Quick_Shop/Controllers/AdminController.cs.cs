using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quick_Shop.Data;
using Quick_Shop.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Quick_Shop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");

        public AdminController(AppDbContext context)
        {
            _context = context;
            // Create img folder if it doesn't exist
            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(_imagePath);
            }
        }

        // GET: Admin Dashboard
        public IActionResult Dashboard()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // GET: Create Product
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile MainImageFile, IFormFileCollection AdditionalImagesFiles, string imageOption, string imagesOption)
        {
            try
            {
                // Remove ModelState validation for Image and Images if uploading files
                if (imageOption == "upload" && MainImageFile != null && MainImageFile.Length > 0)
                {
                    ModelState.Remove("Image");
                }
                if (imagesOption == "upload" && AdditionalImagesFiles != null && AdditionalImagesFiles.Count > 0)
                {
                    ModelState.Remove("Images");
                }

                // Handle Main Image
                if (imageOption == "upload" && MainImageFile != null && MainImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImageFile.FileName);
                    var filePath = Path.Combine(_imagePath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await MainImageFile.CopyToAsync(stream);
                    }
                    product.Image = $"/img/{fileName}";
                    Console.WriteLine($"Main Image uploaded: {product.Image}");
                }
                else if (imageOption == "url" && !string.IsNullOrEmpty(product.Image))
                {
                    Console.WriteLine($"Main Image URL: {product.Image}");
                }
                else
                {
                    product.Image = "/images/fallback-image.jpg";
                    Console.WriteLine("Using fallback image for Main Image");
                }

                // Handle Additional Images
                if (imagesOption == "upload" && AdditionalImagesFiles != null && AdditionalImagesFiles.Count > 0)
                {
                    var imageUrls = new string[AdditionalImagesFiles.Count];
                    for (int i = 0; i < AdditionalImagesFiles.Count; i++)
                    {
                        var file = AdditionalImagesFiles[i];
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(_imagePath, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            imageUrls[i] = $"/img/{fileName}";
                        }
                    }
                    product.Images = JsonSerializer.Serialize(imageUrls);
                    Console.WriteLine($"Additional Images uploaded: {product.Images}");
                }
                else if (imagesOption == "url" && !string.IsNullOrEmpty(product.Images))
                {
                    try
                    {
                        JsonSerializer.Deserialize<string[]>(product.Images);
                        Console.WriteLine($"Additional Images URLs: {product.Images}");
                    }
                    catch
                    {
                        ModelState.AddModelError("Images", "Invalid image URLs format.");
                        Console.WriteLine("Invalid JSON format for Additional Images");
                    }
                }
                else
                {
                    product.Images = "[]";
                    Console.WriteLine("No Additional Images provided, using empty array");
                }

                if (ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is valid, saving product...");
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Product saved successfully");
                    return RedirectToAction(nameof(Dashboard));
                }

                // Log validation errors
                Console.WriteLine("ModelState is invalid:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving product: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"An error occurred while saving the product: {ex.Message}");
                return View(product);
            }
        }

        // GET: Edit Product
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Edit Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile MainImageFile, IFormFileCollection AdditionalImagesFiles, string imageOption, string imagesOption)
        {
            try
            {
                // Remove ModelState validation for Image and Images if uploading files
                if (imageOption == "upload" && MainImageFile != null && MainImageFile.Length > 0)
                {
                    ModelState.Remove("Image");
                }
                if (imagesOption == "upload" && AdditionalImagesFiles != null && AdditionalImagesFiles.Count > 0)
                {
                    ModelState.Remove("Images");
                }

                // Handle Main Image
                if (imageOption == "upload" && MainImageFile != null && MainImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImageFile.FileName);
                    var filePath = Path.Combine(_imagePath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await MainImageFile.CopyToAsync(stream);
                    }
                    product.Image = $"/img/{fileName}";
                    Console.WriteLine($"Main Image uploaded: {product.Image}");
                }
                else if (imageOption == "url" && !string.IsNullOrEmpty(product.Image))
                {
                    Console.WriteLine($"Main Image URL: {product.Image}");
                }
                else if (string.IsNullOrEmpty(product.Image))
                {
                    product.Image = "/images/fallback-image.jpg";
                    Console.WriteLine("Using fallback image for Main Image");
                }

                // Handle Additional Images
                if (imagesOption == "upload" && AdditionalImagesFiles != null && AdditionalImagesFiles.Count > 0)
                {
                    var imageUrls = new string[AdditionalImagesFiles.Count];
                    for (int i = 0; i < AdditionalImagesFiles.Count; i++)
                    {
                        var file = AdditionalImagesFiles[i];
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(_imagePath, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            imageUrls[i] = $"/img/{fileName}";
                        }
                    }
                    product.Images = JsonSerializer.Serialize(imageUrls);
                    Console.WriteLine($"Additional Images uploaded: {product.Images}");
                }
                else if (imagesOption == "url" && !string.IsNullOrEmpty(product.Images))
                {
                    try
                    {
                        JsonSerializer.Deserialize<string[]>(product.Images);
                        Console.WriteLine($"Additional Images URLs: {product.Images}");
                    }
                    catch
                    {
                        ModelState.AddModelError("Images", "Invalid image URLs format.");
                        Console.WriteLine("Invalid JSON format for Additional Images");
                    }
                }
                else if (string.IsNullOrEmpty(product.Images))
                {
                    product.Images = "[]";
                    Console.WriteLine("No Additional Images provided, using empty array");
                }

                if (ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is valid, updating product...");
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Product updated successfully");
                    return RedirectToAction(nameof(Dashboard));
                }

                Console.WriteLine("ModelState is invalid:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"An error occurred while updating the product: {ex.Message}");
                return View(product);
            }
        }

        // GET: Delete Product
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Delete Product
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                    Console.WriteLine("Product deleted successfully");
                }
                return RedirectToAction(nameof(Dashboard));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while deleting the product. Please try again.");
                return RedirectToAction(nameof(Dashboard));
            }
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Quick_Shop.Data.ViewModels
{
    public class NewProductVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        [Required]
        public string Category { get; set; }

        public IFormFile ImageFile { get; set; }
        public List<IFormFile> AdditionalImageFiles { get; set; }

        public string Image { get; set; }
        public string Images { get; set; }

        [Required]
        public string Description { get; set; }

        public int StockQuantity { get; set; }
        public bool IsFeatured { get; set; }
    }
}
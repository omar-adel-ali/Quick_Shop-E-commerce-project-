using Quick_Shop.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace Quick_Shop.Models
{
    public class Product : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        [Required]
        public string Category { get; set; }

        public string Image { get; set; }

        public string Images { get; set; }

        [Required]
        public string Description { get; set; }

        //public int StockQuantity { get; set; } // Added StockQuantity

        //public bool IsFeatured { get; set; } // Added IsFeatured
    }
}
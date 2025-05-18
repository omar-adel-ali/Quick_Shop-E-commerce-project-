using Quick_Shop.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quick_Shop.Data.ViewModels
{
    public class ProductSearchVM
    {
        [Display(Name = "Search")]
        public string SearchString { get; set; }

        [Display(Name = "Category")]
        public List<string> Category { get; set; } = new List<string>();

        [Display(Name = "Max Price")]
        public decimal? MaxPrice { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
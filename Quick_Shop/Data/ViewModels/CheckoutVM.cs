using Quick_Shop.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quick_Shop.Data.ViewModels
{
    public class CheckoutVM
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        public decimal Total { get; set; }

        public List<Cart> CartItems { get; set; } = new List<Cart>();
    }
}
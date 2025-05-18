using Quick_Shop.Models;
using System.Collections.Generic;

namespace Quick_Shop.Data.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<Cart> CartItems { get; set; } = new List<Cart>();
        public decimal CartTotal { get; set; }
    }
}
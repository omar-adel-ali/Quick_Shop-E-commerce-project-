using Quick_Shop.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace Quick_Shop.Models
{
    public class Wishlist : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
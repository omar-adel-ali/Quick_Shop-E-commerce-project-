using Quick_Shop.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace Quick_Shop.Models
{
    public class OrderItem : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public Product Product { get; set; }
    }
}
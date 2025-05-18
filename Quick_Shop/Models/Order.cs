using Quick_Shop.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quick_Shop.Models
{
    public class Order : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Email { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Shipping { get; set; }

        public decimal Total { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public string OrderStatus { get; set; } // Added OrderStatus

        public List<OrderItem> OrderItems { get; set; }
    }
}
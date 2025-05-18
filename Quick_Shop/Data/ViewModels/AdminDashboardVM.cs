using Quick_Shop.Models;
using System;
using System.Collections.Generic;

namespace Quick_Shop.Data.ViewModels
{
    public class AdminDashboardVM
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalSales { get; set; }
        public int PendingOrders { get; set; }
        public Dictionary<DateTime, decimal> SalesData { get; set; } // Sales for last 7 days
        public List<TopSellingProductVM> TopSellingProducts { get; set; }
        public List<Order> RecentOrders { get; set; }
    }

    public class TopSellingProductVM
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
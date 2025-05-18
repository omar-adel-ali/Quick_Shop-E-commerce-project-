using System.Collections.Generic;

namespace Quick_Shop.Data.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
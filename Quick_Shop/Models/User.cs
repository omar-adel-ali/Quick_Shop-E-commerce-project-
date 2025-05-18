using Microsoft.AspNetCore.Identity;

namespace Quick_Shop.Models.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
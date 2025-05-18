using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quick_Shop.Data.ViewModels;
using System.Threading.Tasks;
using System.Linq;

namespace Quick_Shop.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.Select(u => new UserVM
            {
                Id = u.Id,
                Email = u.Email,
                Phone = u.PhoneNumber,
                FullName = u.UserName,
                Roles = new List<string>()
            }).ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(_userManager.Users.First(u => u.Id == user.Id));
                user.Roles = roles.ToList(); // Explicit conversion from IList<string> to List<string>
            }

            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserVM
            {
                Id = user.Id,
                Email = user.Email,
                Phone = user.PhoneNumber,
                FullName = user.UserName,
                Roles = roles.ToList() // Explicit conversion from IList<string> to List<string>
            };

            return View(model);
        }
    }
}
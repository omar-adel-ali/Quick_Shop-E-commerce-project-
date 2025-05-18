using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quick_Shop.Data.ViewModels;
using Quick_Shop.Models.Models;
using System.Threading.Tasks;

namespace Quick_Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields correctly.";
                return View(loginVM);
            }

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user == null)
            {
                TempData["Error"] = "No account found with this email address.";
                return View(loginVM);
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
            if (!passwordCheck)
            {
                TempData["Error"] = "Incorrect password. Please try again.";
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Login failed. Please try again or contact support.";
                return View(loginVM);
            }

            TempData["Success"] = "Logged in successfully!";
            // تحقق من الدور وتوجيه المستخدم
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields correctly.";
                return View(registerVM);
            }

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already registered.";
                return View(registerVM);
            }

            var newUser = new Users
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Customer");
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                TempData["Success"] = "Account created successfully!";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Failed to create account: " + string.Join(", ", newUserResponse.Errors.Select(e => e.Description));
            return View(registerVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "Logged out successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
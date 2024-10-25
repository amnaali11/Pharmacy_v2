using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_v2.Data;
using Pharmacy_v2.DTOs;
using Pharmacy_v2.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pharmacy_v2.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager; 

        public AccountController(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager; 
        }

        public IActionResult Login()
        {
        return View();
        }
        public async Task EnsureRoleExists(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var role = new IdentityRole { Name = roleName };
                await _roleManager.CreateAsync(role);
            }
        }
        public async Task<IActionResult> UnLockUser(string id)
        {
            await EnsureRoleExists("Locked");
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, "Locked");
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllUsers");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return RedirectToAction("GetAllUsers");

        }

        public async Task<IActionResult> LockUser(string id)
        {
            ApplicationUser? User1 = await _userManager.FindByNameAsync(User.Identity.Name);
            if (User != null)
            {
                if (_userManager.IsInRoleAsync(User1, "Locked").Result)
                {
                    return View("Locked");
                }
            }
            await EnsureRoleExists("Locked");
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "Locked");
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllUsers");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return RedirectToAction("GetAllUsers");

        }
        public async Task<IActionResult> ActivateNewAdmin(string id)
        {

            ApplicationUser? User1 = await _userManager.FindByNameAsync(User.Identity.Name);
            if (User != null)
            {
                if (_userManager.IsInRoleAsync(User1, "Locked").Result)
                {
                    return View("Locked");
                }
            }
            await EnsureRoleExists("Admin");
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "Admin");
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllUsers");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return RedirectToAction("GetAllUsers");
        }
        public async Task<IActionResult> GetAllUsers()
        {

            ApplicationUser? User1 = await _userManager.FindByNameAsync(User.Identity.Name);
            if (User != null)
            {
                if (_userManager.IsInRoleAsync(User1, "Locked").Result)
                {
                    return View("Locked");
                }
            }
            // Get the role object for "Admin"
            //var adminRole = await _roleManager.FindByNameAsync("Admin");

            //if (adminRole == null)
            //{
            //    return NotFound("Admin role does not exist.");
            //}

            // Get all users
            var allUsers = _userManager.Users.ToList();
            List<NonAdminUsersDTO> LNonAdminUsersDto = new List<NonAdminUsersDTO>();
            foreach (var user in allUsers)
            {
                // Check if the user has the "Admin" role
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                {
                    LNonAdminUsersDto.Add(new NonAdminUsersDTO()
                    {
                        Id = user.Id,
                        Name = user.UserName,
                        Age = user.Age,
                    });
                }
            }

            return View(LNonAdminUsersDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var role = userRoles.FirstOrDefault() ?? "User";

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.Remember_me
                    };

                
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.Remember_me, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home"); 
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                    return RedirectToAction("Index", "Medicine");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }


        public IActionResult Register()
        {

           return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegisterationDTO account)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    UserName = account.UserName,
                    Email = account.Email,
                    Age = account.Age
                };

                var result = await _userManager.CreateAsync(newUser, account.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("User"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    await _userManager.AddToRoleAsync(newUser, "User");
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(account);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_v2.Data;
using Pharmacy_v2.Models;
using Pharmacy_v2.Repo;
using System.Diagnostics;

namespace Pharmacy_v2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        AppDbContext _context;
        IWebHostEnvironment _webHostEnvironment;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public readonly ICategoryRepository CRepo;
        public HomeController(ILogger<HomeController> logger,AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment, ICategoryRepository _CRepo)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _roleManager = roleManager;
            CRepo = _CRepo;
            _logger = logger;

        }
  

        public async Task<IActionResult> Index(string? search)
        {
            if (User?.Identity?.Name == null)
            {
                return View("Index");
            }
            if (User == null)
            {
                return View("Index");
            }
            else
            {
                ApplicationUser? User1 = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (User1 != null)
                {
                    if (User != null)
                    {
                        if (_userManager.IsInRoleAsync(User1, "Locked").Result)
                        {
                            return View("Locked");
                        }
                    }
                    ViewBag.Search = search;
                    if (string.IsNullOrEmpty(search) == true)
                    {
                        return View("Index", CRepo.GetAll());

                    }
                    else
                    {
                        return View("Index", CRepo.SearchByName(search));
                    }
                }
                else
                {
                    return View("Index");
                }
            }
           
           
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

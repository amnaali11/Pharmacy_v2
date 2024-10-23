
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_v2.Data;
using Pharmacy_v2.DTOs;
using Pharmacy_v2.Models;
using Pharmacy_v2.Repo;
using System.Data;

namespace Pharmacy_v2.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {

        AppDbContext _context;
        IWebHostEnvironment _webHostEnvironment;
        private  UserManager<ApplicationUser> _userManager;
        private  RoleManager<IdentityRole> _roleManager;

        public readonly ICategoryRepository CRepo;
        public CategoriesController(AppDbContext context,UserManager<ApplicationUser> userManager ,RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment,ICategoryRepository _CRepo)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _roleManager = roleManager;
            CRepo= _CRepo;
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
                        Id= user.Id,
                        Name=user.UserName,
                        Age=user.Age,
                    });
                }
            }

            return View(LNonAdminUsersDto); 
        }
        
        [HttpGet]
        public async Task<IActionResult> GetIndexView(string? search)
        {

            ApplicationUser? User1 = await _userManager.FindByNameAsync(User.Identity.Name);
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

        [HttpGet]
        public async Task<IActionResult> GetDetailsView(int id)
        {

            ApplicationUser? User1 = await _userManager.FindByNameAsync(User.Identity.Name);
            if (User != null)
            {
                if (_userManager.IsInRoleAsync(User1, "Locked").Result)
                {
                    return View("Locked");
                }
            }

            Category categ = CRepo.GetMedicineWithCategoryById(id);

                ViewBag.CurrentCateg = categ;
                if (categ == null)
                {
                    return NotFound();
                }
                else
                {
                    return View("Details", categ);
                }

            
          

        }
        [HttpGet]
        public IActionResult GetCreateView()
        {
            return View("Create");
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Category category = CRepo.GetById(id);

            if (category == null)
            {
                return NotFound();
            }
            else
            {
                return View("Edit", category);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditCurrent(Category categ, IFormFile? imageFormFile)
        {

            ApplicationUser? User1 = await _userManager.FindByNameAsync(User.Identity.Name);
            if (User != null)
            {
                if (_userManager.IsInRoleAsync(User1, "Locked").Result)
                {
                    return View("Locked");
                }
            }

            if (ModelState.IsValid == true)
            {
                if (imageFormFile != null)
                {
                    if (categ.ImagePath != "\\images\\No_Image.png")
                    {
                        string oldImgFullPath = _webHostEnvironment.WebRootPath + categ.ImagePath;
                        System.IO.File.Delete(oldImgFullPath);
                    }
                    string imgExtension = Path.GetExtension(imageFormFile.FileName);
                    Guid imgGuid = Guid.NewGuid();
                    string imgName = imgGuid + imgExtension;
                    string imgPath = "\\Images\\" + imgName;
                    categ.ImagePath = imgPath;
                    string imgFullPath = _webHostEnvironment.WebRootPath + imgPath;
                    FileStream imgFileStream = new FileStream(imgFullPath, FileMode.Create);
                    imageFormFile.CopyTo(imgFileStream);
                    imgFileStream.Dispose();

                }
                CRepo.Update(categ);
				TempData["Update"] = "Data Has Been Updated Successfully";


				return RedirectToAction("GetIndexView");
            }
            else
            {
                return View("Edit", categ);
            }
        }
        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Category categ = CRepo.GetById(id);
            ViewBag.CurrentCateg = categ;


            if (categ == null)
            {
                return NotFound();
            }
            else
            {
				return View("Delete", categ);
            }
        }
        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Category category = CRepo.GetByIdSharp(id);

            if (category != null && category.ImagePath != "\\Images\\No_Image.png")
            {
                string imgFullPath = _webHostEnvironment.WebRootPath + category.ImagePath;
                System.IO.File.Delete(imgFullPath);
			}
			TempData["Delete"] = "Data Has Been Deleted Successfully";
			CRepo.Delete(category);

			return RedirectToAction("GetIndexView");
        }
        [HttpPost]
        public async Task<IActionResult> AddNew(Category categ, IFormFile? imageFormFile)
        {

            ApplicationUser? User1 = await _userManager.FindByNameAsync(User.Identity.Name);
            if (User != null)
            {
                if (_userManager.IsInRoleAsync(User1, "Locked").Result)
                {
                    return View("Locked");
                }
            }
            if (ModelState.IsValid == true)
            {
                if (imageFormFile != null)
                {
                    string imgExtension = Path.GetExtension(imageFormFile.FileName);
                    Guid imgGuid = Guid.NewGuid();
                    string imgName = imgGuid + imgExtension;
                    string imgPath = "\\Images\\" + imgName;
                    categ.ImagePath = imgPath;
                    string imgFullPath = _webHostEnvironment.WebRootPath + imgPath;
                    FileStream imgFileStream = new FileStream(imgFullPath, FileMode.Create);
                    imageFormFile.CopyTo(imgFileStream);
                    imgFileStream.Dispose();
					TempData["Create"] = "Data Has Been Created Successfully";
				}
				else
                {
                    categ.ImagePath = "\\Images\\No_Image.png";
                }
                CRepo.Insert(categ);

                return RedirectToAction("GetIndexView");

            }

            else
            {
                return View("Create", categ);
            }
        }
    }
}

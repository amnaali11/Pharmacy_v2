using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy_v2.Data;
using Pharmacy_v2.Models;
using Pharmacy_v2.Repo;
using System.Diagnostics;

namespace Pharmacy_v2.Controllers
{
    public class MedicineController : Controller
    {
        private readonly AppDbContext dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMedicineReposatory Mrepo;
        private readonly UserManager<ApplicationUser> _userManeger;

        public MedicineController(AppDbContext _dbcontext, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IMedicineReposatory _Mrepo)
        {
            dbcontext = _dbcontext;
            _webHostEnvironment = webHostEnvironment;
            Mrepo = _Mrepo;
            _userManeger = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //if (User != null)
            //{
            //    ApplicationUser? user = await _userManeger.FindByNameAsync(User.Identity.Name);
            //    if (user != null)
            //    {
            //        if (_userManeger.IsInRoleAsync(user, "Locked").Result)
            //        {
            //            return View("Locked");
            //        }
            //    }
            //}


            List<Medicine> medicines = Mrepo.GetAll();
            return View(medicines);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(); // Just return the empty form to add a new medicine
        }

        [HttpPost]
        public IActionResult SaveAdd(Medicine medicine, IFormFile? imageFormFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Add image handling if an image is uploaded
                    if (imageFormFile != null)
                    {
                        string imgExtension = Path.GetExtension(imageFormFile.FileName);
                        Guid imgGuid = Guid.NewGuid();
                        string imgName = imgGuid + imgExtension;
                        string imgPath = "\\Images\\" + imgName;
                        medicine.Picture = imgPath;
                        string imgFullPath = _webHostEnvironment.WebRootPath + imgPath;
                        using (FileStream imgFileStream = new FileStream(imgFullPath, FileMode.Create))
                        {
                            imageFormFile.CopyTo(imgFileStream);
                        }
                    }
                    else
                    {
                        // Use default image if no image is uploaded
                        medicine.Picture = "\\Images\\No_Image.png";
                    }
                    TempData["Create"] = "Medicine Has Been Created Successfully";

                    // Add the medicine to the database
                    dbcontext.Medicine.Add(medicine);
                    dbcontext.SaveChanges();
                    return RedirectToAction("Admin");
                }
                catch (DbUpdateException ex)
                {
                    Debug.WriteLine($"Database update error in SaveAdd: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the medicine. Please try again.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in SaveAdd: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                }
            }

            return View("Add", medicine);
        }

        public IActionResult Edit(int id)
        {
            try
            {
                Medicine medicine = dbcontext.Medicine.FirstOrDefault(e => e.Id == id);
                if (medicine == null)
                {
                    return NotFound();
                }
                return View("Edit", medicine);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Edit: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult SaveEdit(int id, Medicine medicine, IFormFile? imageFormFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Medicine med = dbcontext.Medicine.FirstOrDefault(e => e.Id == id);
                    if (med == null)
                    {
                        return NotFound();
                    }

                    // Update the medicine details
                    med.Name = medicine.Name;
                    med.Price = medicine.Price;
                    med.ProductionDate = medicine.ProductionDate;
                    med.ExpiryDate = medicine.ExpiryDate;

                    // Handle image upload
                    if (imageFormFile != null)
                    {
                        if (med.Picture != "\\Images\\No_Image.png")
                        {
                            string oldImgFullPath = _webHostEnvironment.WebRootPath + med.Picture;
                            System.IO.File.Delete(oldImgFullPath);
                        }

                        string imgExtension = Path.GetExtension(imageFormFile.FileName);
                        Guid imgGuid = Guid.NewGuid();
                        string imgName = imgGuid + imgExtension;
                        string imgPath = "\\Images\\" + imgName;
                        med.Picture = imgPath;
                        string imgFullPath = _webHostEnvironment.WebRootPath + imgPath;
                        using (FileStream imgFileStream = new FileStream(imgFullPath, FileMode.Create))
                        {
                            imageFormFile.CopyTo(imgFileStream);
                        }
                    }
                    TempData["Update"] = "Medicine   Has Been Updated Successfully";

                    dbcontext.SaveChanges();
                    return RedirectToAction("Admin");
                }
                catch (DbUpdateException ex)
                {
                    Debug.WriteLine($"Database update error in SaveEdit: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the medicine. Please try again.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in SaveEdit: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                }
            }

            return View("Edit", medicine);
        }

        public IActionResult Admin() // Admin_page // show medicine
        {
            try
            {
                List<Medicine> medicines = dbcontext.Medicine.ToList();
                return View(medicines);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Admin: {ex.Message}");
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Search(string? search)
        {
            ViewBag.Search = search;
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    return View("Admin", dbcontext.Medicine.ToList());
                }
                else
                {
                    return View("Admin", dbcontext.Medicine.Where(c => c.Name.Contains(search)).ToList());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Search: {ex.Message}");
                return View("Error");
            }
        }
        public IActionResult Search2(string? search, int? categoryId)
        {
            ViewBag.Search = search;

            // Get all categories to display in the filter dropdown
            ViewBag.Categories = dbcontext.Categories.ToList();

            try
            {
                // Start building the query for medicines
                var query = dbcontext.Medicine.AsQueryable();

                // Filter by search term if provided
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(m => m.Name.Contains(search));
                }

                // Filter by category if provided
                if (categoryId.HasValue && categoryId > 0)
                {
                    query = query.Where(m => m.CategoryId == categoryId.Value);
                }

                // Execute the query and return the list of filtered medicines
                return View("Index", query.ToList());
            }
            catch (Exception ex)
            {
                // Log the error and return an error view
                Debug.WriteLine($"Error in Search: {ex.Message}");
                return View("Error");
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Medicine medicine = dbcontext.Medicine.Find(id);

                if (medicine == null)
                {
                    return RedirectToAction("Admin");
                }

                // Delete the associated image
                if (medicine.Picture != "\\Images\\No_Image.png")
                {
                    string imgFullPath = _webHostEnvironment.WebRootPath + medicine.Picture;
                    System.IO.File.Delete(imgFullPath);
                }
                TempData["Delete"] = "Medicine Has Been Deleted Successfully";
                dbcontext.Medicine.Remove(medicine);
                dbcontext.SaveChanges();

                return RedirectToAction("Admin");
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"Database update error in Delete: {ex.Message}");
                return RedirectToAction("Admin");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Delete: {ex.Message}");
                return RedirectToAction("Admin");
            }
        }
    }
}
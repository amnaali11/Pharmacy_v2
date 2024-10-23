using Microsoft.EntityFrameworkCore;
using Pharmacy_v2.Data;
using Pharmacy_v2.Models;

namespace Pharmacy_v2.Repo
{
    public class CategoryReposatory : ICategoryRepository
    {
        public AppDbContext context { get; }
        public CategoryReposatory(AppDbContext _context)
        {
            context =_context;
        }


        public void Delete(Category Model)
        {
            context.Categories.Remove(Model);
            context.SaveChanges();

        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category? GetById(int id)
        {
            return context.Categories.FirstOrDefault(x => x.Id == id);
        }


        public void Insert(Category Model)
        {
            context.Categories.Add(Model);
            context.SaveChanges();
        }

        public void Update(Category Model)
        {
            context.Categories.Update(Model);
            context.SaveChanges();
        }

        public IEnumerable<Category> GetCategoriesWithMedicines()
        {
            return context.Categories.Include(x => x.Medicines).ToList();
        }

        public IEnumerable<Category> SearchByName(string? search)
        {
            return context.Categories.Where(c => c.CategoryName.Contains(search)).ToList();
        }

        public Category GetMedicineWithCategoryById(int id)
        {
            return context.Categories.Include(c => c.Medicines).FirstOrDefault(c => c.Id == id);
        }

        public Category? GetByIdSharp(int id)
        {
            return context.Categories.Find(id);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}

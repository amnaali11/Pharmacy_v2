using Pharmacy_v2.Models;

namespace Pharmacy_v2.Repo
{
    public interface ICategoryRepository:GenaricReposatory<Category>
    {
        IEnumerable<Category> GetCategoriesWithMedicines();
        Category GetMedicineWithCategoryById(int id);
    }
}

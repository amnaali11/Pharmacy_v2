using Pharmacy_v2.Models;

namespace Pharmacy_v2.Repo
{
    public interface IMedicineReposatory:GenaricReposatory<Medicine>
    {
        IEnumerable<Medicine> GetMedicinesWithCategories();
    }
}

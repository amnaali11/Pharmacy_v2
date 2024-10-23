using Pharmacy_v2.Models;

namespace Pharmacy_v2.Repo
{
    public interface GenaricReposatory<T>
    {
        List<T> GetAll();
        void Insert(T Model);
        T GetById(int id);
        void Update(T Model);
        void Delete(T Model);
        T? GetByIdSharp(int id);
        IEnumerable<T> SearchByName(string? search);
        void Save();


    }
}

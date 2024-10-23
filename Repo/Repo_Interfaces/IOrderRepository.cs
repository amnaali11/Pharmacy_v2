using Pharmacy_v2.Models;
using Pharmacy_v2.Repo;

namespace Pharmacy_v2.Repos.Repo_Interfaces
{
    public interface IOrderRepository:GenaricReposatory<Order>
    {
        List<Order>? GetOrdersInBag(int id);

    }
}

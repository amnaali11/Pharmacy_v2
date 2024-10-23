using Pharmacy_v2.Models;
using Pharmacy_v2.Repos.Repo_Interfaces;
using Pharmacy_v2.Data;

namespace Pharmacy_v2.Repos
{
    public class OrderRepository : IOrderRepository
    {


        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> GetAll()
        {
            return _context.orders.ToList();
        }

        public Order GetById(int  id)  
        {
            return _context.orders.Where(x => x.Id == id).First();
        }

        public void Insert(Order order)
        {
            _context.orders.Add(order);
        }

        public void Delete(Order order)
        {
            _context.orders.Remove(order);

        }

        public void Update(Order order)
        {
            _context.orders.Update(order);
        }
        public void Save()
        {
            _context.SaveChanges();

        }

        public List<Order>? GetOrdersInBag(int id)  //id_bag
        {
            return _context.orders.Where(x => x.BagId ==id).ToList();
        }

        public Order? GetByIdSharp(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> SearchByName(string? search)
        {
            return _context.orders.Where(x => x.Name == search);
        }
    }
}


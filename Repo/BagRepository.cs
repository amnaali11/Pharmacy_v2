using Microsoft.EntityFrameworkCore;
using Pharmacy_v2.Data;
using Pharmacy_v2.Models;
using Pharmacy_v2.Repos.Repo_Interfaces;
using System;

namespace Pharmacy_v2.Repos
{
    public class BagRepository : IBagRepository
    {

        private readonly AppDbContext _context;

        public BagRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Bag> GetAll()
        {
            return _context.bag.ToList();
        }
        public Bag GetById(int id)
        {
           return _context.bag.Where(x=> x.Id == id).First(); 
        }
        public void Insert(Bag bag)
        {
            _context.bag.Add(bag);
        }
        public void Update(Bag bag)
        {
            _context.bag.Update(bag);

        }
        public void Delete(Bag bag)
        {
            _context.bag.Remove(bag);
        }        

        public void Save()
        {
            _context.SaveChanges();

        }

        public Bag? GetByUserId(string? id) { 
           return _context.bag.FirstOrDefault(x=> x.UserId== id);
        }

        public Bag? GetByIdSharp(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bag> SearchByName(string? search)
        {
            throw new NotImplementedException();
        }

        List<Bag> Repo.GenaricReposatory<Bag>.GetAll()
        {
            throw new NotImplementedException();
        }

        Bag Repo.GenaricReposatory<Bag>.GetById(int id)
        {
            return _context.bag.Where(x => x.Id == id).First();
        }
    }
}

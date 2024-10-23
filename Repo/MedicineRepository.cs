using Microsoft.EntityFrameworkCore;
using Pharmacy_v2.Data;
using Pharmacy_v2.Models;

namespace Pharmacy_v2.Repo
{
    public class MedicineRepository : IMedicineReposatory
    {
        private readonly AppDbContext context;

        public MedicineRepository(AppDbContext _context)
        {
            context = _context;
        }
        public void Delete(Medicine Model)
        {
            context.Medicine.Remove(Model);
            context.SaveChanges();

        }

        public List<Medicine> GetAll()
        {
            return context.Medicine.ToList();
        }

        public Medicine? GetById(int id)
        {
            return context.Medicine.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Medicine> GetMedicinesWithCategories()
        {
            return context.Medicine.Include(x=>x.category).ToList();
        }

        public void Insert(Medicine Model)
        {
            context.Medicine.Add(Model);
            context.SaveChanges();
        }

        public void Update(Medicine Model)
        {
            context.Medicine.Update(Model);
            context.SaveChanges();
        }
        //dbcontext.Medicine.Where(c => c.Name.Contains(search))
        public IEnumerable<Medicine>? SearchByName(string name) 
        {
            return context.Medicine.Where(c => c.Name.Contains(name)).ToList();
        }
        public Medicine GetByIdSharp(int id)
        {
           return context.Medicine.Find(id);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}

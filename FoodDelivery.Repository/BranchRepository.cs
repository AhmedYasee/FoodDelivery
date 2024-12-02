using FoodDelivery.Models;
using FoodDelivery.Repository.Data;
using FoodDelivery.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;

        public BranchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Branch branch)
        {
            _context.Branches.Add(branch);
            Save();
        }

        public void Update(Branch branch)
        {
            _context.Branches.Update(branch);
            Save();
        }

        public void Delete(Branch branch)
        {
            _context.Branches.Remove(branch);
            Save();
        }

        public void Delete(int branchId)
        {
            var branch = _context.Branches.FirstOrDefault(b => b.Id == branchId);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
                Save();
            }
        }

        public Branch Get(int id)
        {
            return _context.Branches.FirstOrDefault(b => b.Id == id);
        }

        public List<Branch> GetAll(string Include = null)
        {
            if (Include == "Manager")
            {
                return _context.Branches.Include(b => b.Manager).ToList();
            }
            return _context.Branches.ToList();
        }

        public IEnumerable<Branch> GetAllWithWarehouses()
        {
            return _context.Branches.Include(b => b.Warehouses).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

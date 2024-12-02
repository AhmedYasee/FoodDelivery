using FoodDelivery.Models;
using System.Collections.Generic;

namespace FoodDelivery.Repository.IRepository
{
    public interface IBranchRepository
    {
        void Add(Branch branch);
        void Update(Branch branch);
        void Delete(Branch branch);
        void Delete(int branchId);
        Branch Get(int id);
        List<Branch> GetAll(string Include = null);
        IEnumerable<Branch> GetAllWithWarehouses();
        void Save();
    }
}

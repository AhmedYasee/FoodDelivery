using FoodDelivery.Models;
using System.Collections.Generic;

namespace FoodDelivery.Repository.IRepository
{
    public interface IWarehouseRepository
    {
        void Add(Warehouse warehouse);
        void Update(Warehouse warehouse);
        void Delete(Warehouse warehouse);
        Warehouse Get(int id);
        List<Warehouse> GetAll();
        void Save();
    }
}

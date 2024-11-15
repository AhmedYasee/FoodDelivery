using FoodDelivery.Models;
using FoodDelivery.Repository.Data;
using FoodDelivery.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Repository
{
    public class UnitOfMeasurementRepository : IUnitOfMeasurementRepository
    {
        private readonly ApplicationDbContext _context;

        public UnitOfMeasurementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(UnitOfMeasurement unitOfMeasurement)
        {
            _context.UnitsOfMeasurement.Add(unitOfMeasurement);
        }

        public List<UnitOfMeasurement> GetAll()
        {
            return _context.UnitsOfMeasurement.ToList();
        }

        public UnitOfMeasurement GetById(int id)
        {
            return _context.UnitsOfMeasurement.FirstOrDefault(u => u.UoMID == id);
        }

        public void Remove(int id)
        {
            var uom = _context.UnitsOfMeasurement.FirstOrDefault(u => u.UoMID == id);
            if (uom != null)
            {
                _context.UnitsOfMeasurement.Remove(uom);
            }
        }

        public void Update(UnitOfMeasurement unitOfMeasurement)
        {
            _context.UnitsOfMeasurement.Update(unitOfMeasurement);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

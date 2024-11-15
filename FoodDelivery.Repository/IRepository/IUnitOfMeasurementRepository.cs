using FoodDelivery.Models;
using System.Collections.Generic;

namespace FoodDelivery.Repository.IRepository
{
    public interface IUnitOfMeasurementRepository
    {
        void Add(UnitOfMeasurement unitOfMeasurement);
        void Update(UnitOfMeasurement unitOfMeasurement);
        void Remove(int id);
        List<UnitOfMeasurement> GetAll();
        UnitOfMeasurement GetById(int id);
        void Save();
    }
}

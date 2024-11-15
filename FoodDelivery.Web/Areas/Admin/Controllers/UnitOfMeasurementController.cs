using FoodDelivery.Models;
using FoodDelivery.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UnitOfMeasurementController : Controller
    {
        private readonly IUnitOfMeasurementRepository _uomRepository;

        public UnitOfMeasurementController(IUnitOfMeasurementRepository uomRepository)
        {
            _uomRepository = uomRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_uomRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UnitOfMeasurement unitOfMeasurement)
        {
            if (ModelState.IsValid)
            {
                _uomRepository.Add(unitOfMeasurement);
                _uomRepository.Save();
                return RedirectToAction("Index");
            }
            return View(unitOfMeasurement);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(_uomRepository.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UnitOfMeasurement unitOfMeasurement)
        {
            if (ModelState.IsValid)
            {
                _uomRepository.Update(unitOfMeasurement);
                _uomRepository.Save();
                return RedirectToAction("Index");
            }
            return View(unitOfMeasurement);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_uomRepository.GetById(id));
        }

        [HttpPost]
        public IActionResult Delete(UnitOfMeasurement unitOfMeasurement)
        {
            if (unitOfMeasurement.UoMID != 0)
            {
                _uomRepository.Remove(unitOfMeasurement.UoMID);  // Pass the ID, not the whole object
                _uomRepository.Save();
                TempData["Success"] = "Deleted Successfully";
            }
            else
            {
                ModelState.AddModelError("", "Cannot Delete This Unit");
            }
            return RedirectToAction("Index");
        }

    }
}

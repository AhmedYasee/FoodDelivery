using FoodDelivery.Repository.IRepository;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeRepository _productTypeRepository;

        public ProductTypeController(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_productTypeRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _productTypeRepository.Add(productType);
                _productTypeRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Create", productType);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(_productTypeRepository.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _productTypeRepository.Update(productType);
                _productTypeRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", productType);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_productTypeRepository.GetById(id));
        }

        [HttpPost]
        public IActionResult Delete(ProductType productType)
        {
            if (productType.TypeID != 0)
            {
                _productTypeRepository.Remove(productType.TypeID);
                _productTypeRepository.Save();
                TempData["Success"] = "Deleted Successfully";
            }
            else
            {
                ModelState.AddModelError("", "Cannot Delete This Product Type");
            }
            return RedirectToAction("Index");
        }
    }
}

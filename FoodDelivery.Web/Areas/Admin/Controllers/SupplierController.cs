using FoodDelivery.Models;
using FoodDelivery.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        // Index Page
        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Modules/Purchase/Supplier/Index.cshtml");
        }

        // Add or Edit Supplier (GET)
        public IActionResult AddSupplier(int id = 0)
        {
            Supplier supplier = id == 0 ? new Supplier() : _supplierRepository.GetSupplierById(id);
            if (supplier == null) return NotFound();

            return View("~/Areas/Admin/Views/Modules/Purchase/Supplier/AddSupplier.cshtml", supplier);
        }

        // Add or Edit Supplier (POST)
        [HttpPost]
        public IActionResult AddSupplier(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                // Return the AddSupplier view with the correct path
                return View("~/Areas/Admin/Views/Modules/Purchase/Supplier/AddSupplier.cshtml", supplier);
            }

            if (supplier.Id == 0)
            {
                _supplierRepository.AddSupplier(supplier);
            }
            else
            {
                _supplierRepository.UpdateSupplier(supplier);
            }

            _supplierRepository.SaveChanges();
            return RedirectToAction("Index");
        }

        // Supplier Info Page
        public IActionResult SupplierInfo(int id)
        {
            Supplier supplier = _supplierRepository.GetSupplierById(id);
            if (supplier == null) return NotFound();

            return View("~/Areas/Admin/Views/Modules/Purchase/Supplier/SupplierInfo.cshtml", supplier);
        }

        // Get All Suppliers (for DataTables)
        [HttpGet]
        public IActionResult GetAll()
        {
            var suppliers = _supplierRepository.GetAllSuppliers();
            return Json(new { data = suppliers });
        }

        // Delete Supplier
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var supplier = _supplierRepository.GetSupplierById(id);
            if (supplier == null)
            {
                return Json(new { success = false, message = "Supplier not found." });
            }

            _supplierRepository.DeleteSupplier(id);
            _supplierRepository.SaveChanges();

            return Json(new { success = true, message = "Supplier deleted successfully." });
        }
    }
}

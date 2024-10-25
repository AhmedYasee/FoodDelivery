using FoodDelivery.Models;
using FoodDelivery.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IBranchRepository _branchRepository;

        public WarehouseController(IWarehouseRepository warehouseRepository, IBranchRepository branchRepository)
        {
            _warehouseRepository = warehouseRepository;
            _branchRepository = branchRepository;
        }

        // GET: WarehouseController
        // Index action
        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Modules/Inventory/Warehouses/Index.cshtml");
        }

        // Add or Edit action (GET)
        [HttpGet]
        public IActionResult AddWarehouse(int id = 0)
        {
            var branches = _branchRepository.GetAll().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.BranchName
            });

            ViewBag.Branches = new SelectList(branches, "Value", "Text");

            Warehouse warehouse = new Warehouse();

            // If 'id' is not 0, we are editing an existing warehouse
            if (id != 0)
            {
                warehouse = _warehouseRepository.Get(id);
                if (warehouse == null)
                {
                    return NotFound();
                }

                // Set the title for editing
                ViewData["Title"] = "Edit Warehouse";
            }
            else
            {
                // Set the title for creating a new warehouse
                ViewData["Title"] = "Create Warehouse";
            }

            return View("~/Areas/Admin/Views/Modules/Inventory/Warehouses/AddWarehouse.cshtml", warehouse);
        }


        // POST: Add or Edit Warehouse
        [HttpPost]
        public IActionResult AddWarehouse(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                if (warehouse.Id == 0)
                {
                    _warehouseRepository.Add(warehouse);
                }
                else
                {
                    _warehouseRepository.Update(warehouse);
                }
                _warehouseRepository.Save();
                return RedirectToAction("Index");
            }

            var branches = _branchRepository.GetAll().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.BranchName
            });
            ViewBag.Branches = new SelectList(branches, "Value", "Text");

            return View(warehouse);
        }

        // DELETE: Delete Warehouse
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var warehouse = _warehouseRepository.Get(id);
            if (warehouse == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _warehouseRepository.Delete(warehouse);
            _warehouseRepository.Save();
            return Json(new { success = true, message = "Deleted successfully" });
        }

        // GET: All Warehouses for Datatable
        public IActionResult GetAll()
        {
            var warehouses = _warehouseRepository.GetAll();
            return Json(new { data = warehouses });
        }
    }
}

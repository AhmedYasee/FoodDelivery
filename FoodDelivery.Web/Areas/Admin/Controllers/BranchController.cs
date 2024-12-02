using FoodDelivery.Models;
using FoodDelivery.Repository;
using FoodDelivery.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IApplicationUserRepository _userRepository;

        public BranchController(IBranchRepository branchRepository, IWarehouseRepository warehouseRepository, IApplicationUserRepository userRepository)
        {
            _branchRepository = branchRepository;
            _warehouseRepository = warehouseRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Modules/Inventory/BranchManagement/Index.cshtml", _branchRepository.GetAll("Manager"));
        }

        [HttpGet]
        public IActionResult GetAllBranchesWithWarehouses()
        {
            var branches = _branchRepository.GetAllWithWarehouses();
            var result = branches.Select(b => new
            {
                BranchId = b.Id,
                BranchName = b.BranchName,
                Warehouses = b.Warehouses.Select(w => new
                {
                    WarehouseId = w.Id,
                    WarehouseName = w.WarehouseName
                })
            });

            return Json(result);
        }


        public IActionResult AddBranch(int id = 0)
        {
            var adminUsers = _userRepository.GetUsersByRole("Admin");
            var managerUsers = _userRepository.GetUsersByRole("Manager");
            var combinedUsers = adminUsers.Concat(managerUsers).ToList();

            ViewBag.Managers = new SelectList(combinedUsers, "Id", "Name");

            Branch branch = new Branch();
            if (id != 0)
            {
                branch = _branchRepository.Get(id);
                if (branch == null) return NotFound();
            }
            return View("~/Areas/Admin/Views/Modules/Inventory/BranchManagement/AddBranch.cshtml", branch);
        }

        [HttpPost]
        public IActionResult AddBranch(Branch branch)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(branch.Manager.Id))
                {
                    branch.Manager = _userRepository.Get(branch.Manager.Id);
                }

                if (branch.Id == 0)
                {
                    _branchRepository.Add(branch);
                }
                else
                {
                    _branchRepository.Update(branch);
                }

                _branchRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.Managers = new SelectList(_userRepository.GetUsersByRole("Manager").Concat(_userRepository.GetUsersByRole("Admin")), "Id", "Name");
            return View("~/Areas/Admin/Views/Modules/Inventory/BranchManagement/AddBranch.cshtml", branch);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var branch = _branchRepository.Get(id);
            if (branch == null)
            {
                return Json(new { success = false, message = "Error while deleting. Branch not found." });
            }

            var linkedWarehouses = _warehouseRepository.GetByBranch(id);
            if (linkedWarehouses.Any())
            {
                return Json(new { success = false, message = "Cannot delete branch because it is linked to one or more warehouses." });
            }

            _branchRepository.Delete(branch);
            _branchRepository.Save();

            return Json(new { success = true, message = "Branch deleted successfully." });
        }

        public IActionResult GetAll()
        {
            return Json(new { data = _branchRepository.GetAll("Manager") });
        }
    }
}

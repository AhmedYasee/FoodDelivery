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


        public IActionResult AddBranch(int id = 0)
        {
            // Fetch both Admins and Managers
            var adminUsers = _userRepository.GetUsersByRole("Admin");
            var managerUsers = _userRepository.GetUsersByRole("Manager");

            // Combine both lists
            var combinedUsers = adminUsers.Concat(managerUsers).ToList();

            // Pass combined list to the ViewBag
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
                // Ensure that the Manager is correctly fetched from the database
                if (!string.IsNullOrEmpty(branch.Manager.Id))
                {
                    // Fetch the existing ApplicationUser for Manager
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


        // DELETE: Branch
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var branch = _branchRepository.Get(id);
            if (branch == null)
            {
                return Json(new { success = false, message = "Error while deleting. Branch not found." });
            }

            // Check if there are warehouses linked to this branch
            var linkedWarehouses = _warehouseRepository.GetAll().Where(w => w.BranchId == id).ToList();
            if (linkedWarehouses.Any())
            {
                return Json(new { success = false, message = "Cannot delete branch because it is linked to one or more warehouses." });
            }

            // If no linked warehouses, proceed with the deletion
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

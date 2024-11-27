using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")] // Associate the controller with the Admin area
    public class GeneralConfigurationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BranchManagement()
        {
            // Return the BranchManagement view from the correct location
            return View("~/Areas/Admin/Views/Modules/Inventory/BranchManagement/Index.cshtml");
        }
    }
}

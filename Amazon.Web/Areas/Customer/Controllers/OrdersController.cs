using Microsoft.AspNetCore.Mvc;

namespace Amazon.Web.Areas.Customer.Controllers
{
	public class OrdersController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

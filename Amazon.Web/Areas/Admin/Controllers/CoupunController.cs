using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CoupunController : Controller
    {
        private readonly ICoupunRepositoy _coupunRepo;

        public CoupunController(ICoupunRepositoy coupunRepo)
        {
            _coupunRepo = coupunRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            if (id == 0 || id is null)
                return View();
            else
                return View(_coupunRepo.Get((int)id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Coupun coupun)
        {
            ModelState.ClearValidationState("CoupunID");
            ModelState.MarkFieldValid("CoupunID");

            if (ModelState.IsValid)
            {
                //Check Update or Create
                if (coupun.CoupunID == 0)
                {
                    _coupunRepo.Add(coupun);
                    TempData["success"] = "Created Successfully";
                }
                else
                {
                    _coupunRepo.Update(coupun);
                    TempData["success"] = "Updated Successfully";
                }
                _coupunRepo.Save();
                return RedirectToAction("Index");
            }
            return View("Upsert", coupun);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0) return Json(new {success = false, message= "Error Operation Failed!"});
            _coupunRepo.Remove(id);
            _coupunRepo.Save();
            return Json(new { success = true, message = "Operation Successfully!" });
        }


        #region APIS
        public IActionResult GetAll()
        {
            List<Coupun> coupuns = _coupunRepo.GetAll();
            return Json(new { data = coupuns });
        }
        [HttpPost]
        public IActionResult Activate([FromBody]int id)
        {
            Coupun coupun = _coupunRepo.Get(id);
            if (coupun is null) return Json(new { success = false, message = "Error Can't Operate!"});
            coupun.IsActive = !coupun.IsActive;
            _coupunRepo.Update(coupun);
            _coupunRepo.Save();
            return Json(new { success = true, message = "Operation Successfully" });
        }
        #endregion
    }
}

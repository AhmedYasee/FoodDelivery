using Amazon.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using NuGet.Packaging.Signing;

namespace Amazon.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment _web;

        public ProductController(IProductRepository productRepository, ApplicationDbContext _context, IWebHostEnvironment web)
        {
            this.productRepository = productRepository;
            context = _context;
            this._web = web;
        }
        // GET: ProductController
        public IActionResult Index()
        {
            return View(productRepository.GetAll("Category"));
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        [HttpGet]
        public IActionResult Upsert(int id)
        {
            ViewBag.Categories = context.Categories.ToList();
            ProductVM productVM = new ProductVM(); 
            if(id != 0)
            {
                Product product = productRepository.Get(id);
                productVM.ProductID = id;
                productVM.Name = product.Name;
                productVM.Price = product.Price;
                productVM.Description = product.Description;
                return View(productVM);
            }
            else
            {
            }
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM)
        {
            ViewBag.Categories = context.Categories.ToList();
            if (ModelState.IsValid)
            {
                Product product;
                if (productVM.ProductID == 0) {
                    product = new()
                    {
                        Name = productVM.Name,
                        Price = productVM.Price,
                        Description = productVM.Description,
                        CategoryId = productVM.CategoryId,
                    };
                    productRepository.Add(product);
                    productRepository.Save();
                } else {
                    product = productRepository.Get(productVM.ProductID);
                    product.CategoryId = productVM.CategoryId;
                    product.Name = productVM.Name;
                    product.Description = productVM.Description;
                    product.Price = productVM.Price;
                    var images = context.ProductImages.Where(i => i.ProductId == product.ProductID).ToList();
                    foreach (var img in images)
                    {
                        var path = Path.Combine(_web.WebRootPath, img.ImageUrl);
                        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                        context.Remove(img);
                    }
                }
                List<ProductImages> imgs = new List<ProductImages>();
                string[] allowedExt = { ".jpg", ".png",".jpeg" };
                double MaxInMB = 2 * 1024 * 1024;
                bool isValid = true;

                foreach (var file in productVM.files) {

                    foreach (var item in allowedExt)
                    {
                        string extension = Path.GetExtension(file.FileName).ToLower();
                        if (extension.Contains(item))
                        {
                            if (file.Length <= MaxInMB)
                            {
                                string randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".jpg";
                                string path = Path.Combine(_web.WebRootPath, "Images", randomName);
                                using (FileStream stream = new FileStream(path, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }
                                ProductImages productImage = new ProductImages
                                {
                                    ImageUrl = $"Images/{randomName}",
                                    ProductId = product.ProductID,
                                };
                                imgs.Add(productImage);
                                isValid = true;
                            }
                            else
                            {
                                ModelState.AddModelError("", "Img Size Should Be Less Than 2 MB");
                                productRepository.Delete(product);
                                productRepository.Save();
                                return View(productVM);
                            }
                            break;
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                }
                if (!isValid)
                {
                    ModelState.AddModelError("", "File is Not Valid");
                    productRepository.Delete(product);
                    productRepository.Save();
                    return View(productVM);
                }
                product.ProductImages.AddRange(imgs);
                productRepository.Update(product);
                productRepository.Save();
                return RedirectToAction("Index");
            }
            return View(productVM);
        }
        [HttpDelete]

        public IActionResult Delete(int id)
        {
            var product = productRepository.Get(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var prodImgs = context.ProductImages.Where(i => i.ProductId == id).ToList();
            foreach (var item in prodImgs)
            {
                string path = Path.Combine(_web.WebRootPath, item.ImageUrl);

                if (System.IO.File.Exists(path))
                     System.IO.File.Delete(path);

            }
            productRepository.Delete(product);
            productRepository.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        #region APIS
        public IActionResult GetAll()
        {
            List<Product> products = productRepository.GetAll("Category");
            return Json(new { data = products });
        }

		public IActionResult GetImages(int id)
		{
			var images = context.ProductImages.Where(p => p.ProductId == id).ToList();
			return PartialView("_GetImages",images);
		}
		#endregion
	}
}

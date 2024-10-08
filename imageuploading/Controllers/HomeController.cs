using imageuploading.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace imageuploading.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImageUploadingContext _Context;
        private readonly IWebHostEnvironment _env;
        public HomeController(ImageUploadingContext context, IWebHostEnvironment env)
        {
            this._Context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var listOfProducts = _Context.Products.ToList();
            return View(listOfProducts);
        }
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(viewProductModel product)
        {

            string filename = "";
            if (product.photo != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "Images");
                filename = product.photo.FileName;
                string filepath = Path.Combine(folder, filename);
                product.photo.CopyTo(new FileStream(filepath, FileMode.Create));


                Product p = new Product()
                {
                    Name = product.Name,
                    Price = product.Price,
                    ImagePath = product.photo.FileName
                };

                _Context.Products.Add(p);
                _Context.SaveChanges();
                TempData["success"] = "Product Created Successfully!";
                return RedirectToAction("Index");
            }
            return View();
   

        }



    }

}

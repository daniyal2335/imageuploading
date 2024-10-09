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
                var ext = Path.GetExtension(product.photo.FileName);
                var size = product.photo.Length;
                if(ext.Equals(".jpg") || ext.Equals(".png") || ext.Equals(".Webp") || ext.Equals(".jpeg")) {

                    if (size < 1000000)
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
                    else
                    {
                        TempData["sizeErr"] = "Size must be less than 1 mb";
                    }

                }
                else
                {
                    TempData["extError"] = "only Jpg,Png,JPEG And Webp images allowed";
                }
                
            }
            return View();
   

        }
        public IActionResult edit(int Id)
        {
            var product = _Context.Products.Find(Id);
            if (product == null)
            {
                return NotFound();
            }

           
            var viewModel = new viewProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            return View(viewModel); 
        }

        [HttpPost]
     
        public IActionResult edit(int Id, viewProductModel p)
        {
            var product = _Context.Products.Find(Id);
            if (product == null)
            {
                return NotFound();
            }

           
            product.Name = p.Name;
            product.Price = p.Price;

            
            if (p.photo != null)
            {
                var ext = Path.GetExtension(p.photo.FileName);
                if (ext.Equals(".jpg") || ext.Equals(".png") || ext.Equals(".Webp") || ext.Equals(".jpeg"))
                {
                    string folder = Path.Combine(_env.WebRootPath, "Images");
                    string filename = p.photo.FileName;
                    string filepath = Path.Combine(folder, filename);
                    p.photo.CopyTo(new FileStream(filepath, FileMode.Create));

      
                    product.ImagePath = filename;
                }
            }

            _Context.SaveChanges();
            return RedirectToAction("Index");
        }



        public IActionResult delete(int id)
        {
          var a= _Context.Products.Find(id);
            _Context.Products.Remove(a);
            _Context.SaveChanges();            
            return RedirectToAction("Index");
        }
    }

}

using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using DXWebApplication4.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevExtremeAspNetCore.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly FirebaseService _firebaseService;

        public ProductController(AppDbContext context, FirebaseService firebaseService)
        {
            _context = context;
            _firebaseService = firebaseService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            ViewBag.Products = products; 
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Colors = _context.Colors.ToList();

            return View(new ProductCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _context.Products.ToList();
                ViewBag.Sizes = _context.Sizes.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                return View("Index", model);
            }

            var product = new ProductModels
            {
                TenPro = model.TenPro,
                ProductVariants = new List<ProductVariant>()
            };
              
            foreach (var variantVm in model.Variants)
            {
                var variant = new ProductVariant
                {
                    Idsize = variantVm.IDSize,
                    Idcolor = variantVm.IDColor,
                    Images = new List<Image>()
                };

                if (variantVm.Images != null)
                {
                    for (int i = 0; i < variantVm.Images.Count; i++)
                    {
                        var file = variantVm.Images[i];
                        if (file != null && file.Length > 0)
                        {
                            using var stream = file.OpenReadStream();
                            var url = await _firebaseService.UploadFileAsync(stream, file.FileName, "products");

                            variant.Images.Add(new Image
                            {
                                Url = url,
                                NoiDung = variantVm.Notes != null && variantVm.Notes.Count > i
                                          ? variantVm.Notes[i]
                                          : null
                            });
                        }
                    }
                }

                product.ProductVariants.Add(variant);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }
    }
}

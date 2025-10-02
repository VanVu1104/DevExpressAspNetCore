using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using DXWebApplication4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var products = _context.Products
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.Images)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.ListNpls)
                        .ThenInclude(l => l.IdnplNavigation)
                .ToList();

            ViewBag.Products = products;
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.NPLs = _context.Npls.ToList(); // thêm NPL

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
                ViewBag.NPLs = _context.Npls.ToList();
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
                    Images = new List<Image>(),
                    ListNpls = new List<ListNpl>()
                };

                // Upload ảnh variant
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

                // Xử lý NPL
                if (variantVm.NPLs != null)
                {
                    foreach (var nplVm in variantVm.NPLs)
                    {
                        Npl npl;

                        if (nplVm.IDNPL.HasValue && nplVm.IDNPL.Value > 0)
                        {
                            // chọn NPL có sẵn
                            npl = await _context.Npls.FindAsync(nplVm.IDNPL.Value);
                        }
                        else
                        {
                            // tạo mới NPL
                            npl = new Npl
                            {
                                TenNpl = nplVm.TenNPL,
                                ColorNpl = nplVm.ColorNPL,
                                KhoVai = nplVm.KhoVai,
                                Loai = nplVm.Loai,
                                DonVi = nplVm.DonVi,
                                SoLuong = nplVm.SoLuong
                            };

                            _context.Npls.Add(npl);
                            await _context.SaveChangesAsync();

                            // upload file & ảnh note
                            if ((nplVm.Files != null && nplVm.Files.Any()) ||
                                (nplVm.Images != null && nplVm.Images.Any()))
                            {
                                foreach (var file in nplVm.Files ?? new List<IFormFile>())
                                {
                                    using var stream = file.OpenReadStream();
                                    var url = await _firebaseService.UploadFileAsync(stream, file.FileName, "npl/files");
                                    _context.NoteNpls.Add(new NoteNpl
                                    {
                                        Idnpl = npl.Idnpl,
                                        Urlfile = url,
                                        NoiDung = nplVm.NoiDung
                                    });
                                }

                                foreach (var img in nplVm.Images ?? new List<IFormFile>())
                                {
                                    using var stream = img.OpenReadStream();
                                    var url = await _firebaseService.UploadFileAsync(stream, img.FileName, "npl/images");
                                    _context.NoteNpls.Add(new NoteNpl
                                    {
                                        Idnpl = npl.Idnpl,
                                        Urlimage = url,
                                        NoiDung = nplVm.NoiDung
                                    });
                                }
                            }
                        }

                        // Tạo bản ghi ListNPL
                        variant.ListNpls.Add(new ListNpl
                        {
                            Idnpl = npl.Idnpl,
                            IdvariantNavigation = variant
                        });
                    }
                }

                product.ProductVariants.Add(variant);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thêm sản phẩm + NPL thành công!";
            return RedirectToAction("Index");
        }

    }
}

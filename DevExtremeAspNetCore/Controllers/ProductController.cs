using ClosedXML.Excel;
using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using DXWebApplication4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

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
            ViewBag.NPLs = _context.Npls.ToList(); 
            return View(new ProductCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["Message"] = "Vui lòng chọn file Excel.";
                return RedirectToAction("Index");
            }

            using var stream = new MemoryStream();
            await excelFile.CopyToAsync(stream);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1); // Sheet đầu tiên
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // bỏ header

            foreach (var row in rows)
            {
                var productName = row.Cell(1).GetValue<string>();
                var sizeName = row.Cell(2).GetValue<string>();
                var colorName = row.Cell(3).GetValue<string>();
                var imageUrl = row.Cell(4).GetValue<string>();

                var nplName = row.Cell(5).GetValue<string>();
                var nplColor = row.Cell(6).GetValue<string>();
                var loai = row.Cell(7).GetValue<string>();
                var donVi = row.Cell(8).GetValue<string>();
                var soLuong = row.Cell(9).GetValue<int>();
                var note = row.Cell(10).GetValue<string>();

                // --- Product ---
                var product = _context.Products
                    .Include(p => p.ProductVariants)
                    .FirstOrDefault(p => p.TenPro == productName);

                if (product == null)
                {
                    product = new ProductModels { TenPro = productName };
                    _context.Products.Add(product);
                }

                // --- Variant ---
                var size = _context.Sizes.FirstOrDefault(s => s.TenSize == sizeName);
                if (size == null && !string.IsNullOrWhiteSpace(sizeName))
                {
                    size = new Size { TenSize = sizeName };
                    _context.Sizes.Add(size);
                    await _context.SaveChangesAsync();
                }

                var color = _context.Colors.FirstOrDefault(c => c.TenColor == colorName);
                if (color == null && !string.IsNullOrWhiteSpace(colorName))
                {
                    color = new Color { TenColor = colorName };
                    _context.Colors.Add(color);
                    await _context.SaveChangesAsync();
                }

                var variant = product.ProductVariants
                    .FirstOrDefault(v => v.Idsize == size?.Idsize && v.Idcolor == color?.Idcolor);

                if (variant == null)
                {
                    variant = new ProductVariant
                    {
                        Idsize = size?.Idsize,
                        Idcolor = color?.Idcolor,
                        Images = new List<Image>(),
                        ListNpls = new List<ListNpl>()
                    };
                    product.ProductVariants.Add(variant);
                }
                // --- NPL ---
                var npl = _context.Npls.FirstOrDefault(n => n.TenNpl == nplName);
                if (npl == null)
                {
                    npl = new Npl
                    {
                        TenNpl = nplName,
                        ColorNpl = nplColor,
                        Loai = loai,
                        DonVi = donVi,
                        SoLuong = soLuong
                    };
                    _context.Npls.Add(npl);
                    await _context.SaveChangesAsync();
                }

                var listNpl = new ListNpl
                {
                    Idnpl = npl.Idnpl,
                    IdvariantNavigation = variant
                };
                _context.ListNpls.Add(listNpl);

                if (!string.IsNullOrEmpty(note))
                {
                    _context.NoteNpls.Add(new NoteNpl
                    {
                        Idnpl = npl.Idnpl,
                        NoiDung = note
                    });
                }
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Import dữ liệu từ Excel thành công!";
            return RedirectToAction("Index");
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

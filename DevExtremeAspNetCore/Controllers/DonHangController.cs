using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;

namespace DevExtremeAspNetCore.Controllers
{
    public class DonHangController : Controller
    {
        private readonly AppDbContext _db;

        public DonHangController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var donHangs = await _db.DonHangs
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdvariantNavigation)
                        .ThenInclude(pv => pv.IdproNavigation)
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdvariantNavigation)
                        .ThenInclude(pv => pv.IdcolorNavigation)
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdvariantNavigation)
                        .ThenInclude(pv => pv.IdsizeNavigation)
                .ToListAsync();

            var viewModel = donHangs.SelectMany(dh => dh.Ctdhs, (dh, ctdh) => new
            {
                dh.Iddh,
                dh.NgayDat,
                dh.KhachHang,
                ProductName = ctdh.IdvariantNavigation.IdproNavigation.TenPro,
                Color = ctdh.IdvariantNavigation.IdcolorNavigation.TenColor,
                Size = ctdh.IdvariantNavigation.IdsizeNavigation.TenSize,
                SoLuong = ctdh.SoLuong,
                Images = ctdh.IdvariantNavigation.Images.Select(img => new DonHangImage
                {
                    Url = img.Url,
                    Caption = img.NoiDung
                }).ToList()
            })
            .GroupBy(x => new { x.Iddh, x.NgayDat, x.KhachHang, x.ProductName, x.Color })
            .Select(g => new DonHangViewModel
            {
                IDDH = g.Key.Iddh,
                NgayDat = g.Key.NgayDat.HasValue ? g.Key.NgayDat.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                KhachHang = g.Key.KhachHang,
                ProductName = g.Key.ProductName,
                Color = g.Key.Color,
                Images = g.SelectMany(x => x.Images).ToList(),
                S = g.Where(x => x.Size == "XXS").Sum(x => x.SoLuong ?? 0),
                M = g.Where(x => x.Size == "XSM").Sum(x => x.SoLuong ?? 0),
                L = g.Where(x => x.Size == "SM").Sum(x => x.SoLuong ?? 0),
                XL = g.Where(x => x.Size == "MED").Sum(x => x.SoLuong ?? 0),
                //LRG = g.Where(x => x.Size == "LRG").Sum(x => x.SoLuong ?? 0),
                //XLG = g.Where(x => x.Size == "XLG").Sum(x => x.SoLuong ?? 0),
                //XXL = g.Where(x => x.Size == "XXL").Sum(x => x.SoLuong ?? 0)
            })
            .ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var donHangs = await _db.DonHangs
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdvariantNavigation)
                        .ThenInclude(pv => pv.IdproNavigation)
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdvariantNavigation)
                        .ThenInclude(pv => pv.IdcolorNavigation)
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdvariantNavigation)
                        .ThenInclude(pv => pv.IdsizeNavigation)
                .ToListAsync();

            var query = donHangs.SelectMany(dh => dh.Ctdhs, (dh, ctdh) => new
            {
                dh.Iddh,
                dh.NgayDat,
                dh.KhachHang,
                ProductName = ctdh.IdvariantNavigation.IdproNavigation.TenPro,
                Color = ctdh.IdvariantNavigation.IdcolorNavigation.TenColor,
                Size = ctdh.IdvariantNavigation.IdsizeNavigation.TenSize,
                SoLuong = ctdh.SoLuong,
                Images = ctdh.IdvariantNavigation.Images.Select(img => new DonHangImage
                {
                    Url = img.Url,
                    Caption = img.NoiDung
                }).ToList()
            }).ToList();

            var grouped = query
                .GroupBy(x => new { x.Iddh, x.NgayDat, x.KhachHang, x.ProductName, x.Color })
                .Select(g => new DonHangViewModel
                {
                    IDDH = g.Key.Iddh,
                    NgayDat = g.Key.NgayDat.HasValue ? g.Key.NgayDat.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                    KhachHang = g.Key.KhachHang,
                    ProductName = g.Key.ProductName,
                    Color = g.Key.Color,
                    Images = g.SelectMany(x => x.Images).ToList(),
                    S = g.Where(x => x.Size == "S").Sum(x => x.SoLuong ?? 0),
                    M = g.Where(x => x.Size == "M").Sum(x => x.SoLuong ?? 0),
                    L = g.Where(x => x.Size == "L").Sum(x => x.SoLuong ?? 0),
                    XL = g.Where(x => x.Size == "XL").Sum(x => x.SoLuong ?? 0),
                    //LRG = g.Where(x => x.Size == "LRG").Sum(x => x.SoLuong ?? 0),
                    //XLG = g.Where(x => x.Size == "XLG").Sum(x => x.SoLuong ?? 0),
                    //XXL = g.Where(x => x.Size == "XXL").Sum(x => x.SoLuong ?? 0)
                })
                .Where(dh => dh.TongSoLuong > 0)
                .ToList();

            return Json(grouped);
        }
    }
}

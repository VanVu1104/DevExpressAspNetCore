using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using System.Collections.Generic;

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
            var query = from dh in _db.DonHangs
                        join ct in _db.Ctdhs on dh.Iddh equals ct.Iddh
                        join p in _db.Products on ct.Idpro equals p.Idpro
                        join s in _db.Sizes on ct.Idsize equals s.Idsize
                        select new
                        {
                            dh.Iddh,
                            dh.KhachHang,
                            dh.NgayDat,
                            TenSanPham = p.TenPro + " - " + s.TenSize,
                            SoLuong = ct.Soluong ?? 0
                        };

            var result = await query
                .GroupBy(x => new { x.Iddh, x.KhachHang, x.NgayDat, x.TenSanPham })
                .Select(g => new DonHangViewModel
                {
                    IDDH = g.Key.Iddh,
                    TenKhachHang = g.Key.KhachHang,
                    NgayDat = g.Key.NgayDat.HasValue
                        ? g.Key.NgayDat.Value.ToDateTime(TimeOnly.MinValue)
                        : DateTime.MinValue,
                    TenSanPham = g.Key.TenSanPham,
                    TongSoLuong = g.Sum(x => x.SoLuong)
                })
                .ToListAsync();

            return View(result);
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = from dh in _db.DonHangs
                        join ct in _db.Ctdhs on dh.Iddh equals ct.Iddh
                        join v in _db.ProductVariants
                             on new { ct.Idpro, ct.Idsize, ct.Idcolor }
                             equals new { v.Idpro, v.Idsize, v.Idcolor }
                        join p in _db.Products on v.Idpro equals p.Idpro
                        select new
                        {
                            dh.Iddh,
                            dh.KhachHang,
                            dh.NgayDat,
                            TenSanPham = p.TenPro,
                            SoLuong = ct.Soluong ?? 0
                        };

            var result = await query
                .GroupBy(x => new { x.Iddh, x.KhachHang, x.NgayDat, x.TenSanPham })
                .Select(g => new DonHangViewModel
                {
                    IDDH = g.Key.Iddh,
                    TenKhachHang = g.Key.KhachHang,
                    NgayDat = g.Key.NgayDat.HasValue
                        ? g.Key.NgayDat.Value.ToDateTime(TimeOnly.MinValue)
                        : DateTime.MinValue,
                    TenSanPham = g.Key.TenSanPham,
                    TongSoLuong = g.Sum(x => x.SoLuong)
                })
                .ToListAsync();

            return View("Index", result);
        }
    
    }
}

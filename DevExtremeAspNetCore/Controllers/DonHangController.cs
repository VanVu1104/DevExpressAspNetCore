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
            var donHangs = await _db.DonHangs
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdvariantNavigation)
                        .ThenInclude(v => v.IdproNavigation)
                .ToListAsync();

            var result = donHangs.SelectMany(dh => dh.Ctdhs, (dh, ctdh) => new
            {
                dh.Iddh,
                dh.KhachHang,
                dh.NgayDat,
                TenSanPham = ctdh.IdvariantNavigation.IdproNavigation.TenPro,
                SoLuong = ctdh.SoLuong ?? 0
            })
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
            .ToList();

            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var donHangs = await _db.DonHangs
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdvariantNavigation)
                        .ThenInclude(v => v.IdproNavigation)
                .ToListAsync();

            var result = donHangs.SelectMany(dh => dh.Ctdhs, (dh, ctdh) => new
            {
                dh.Iddh,
                dh.KhachHang,
                dh.NgayDat,
                TenSanPham = ctdh.IdvariantNavigation.IdproNavigation.TenPro,
                SoLuong = ctdh.SoLuong ?? 0
            })
            .GroupBy(x => new { x.Iddh, x.KhachHang, x.NgayDat, x.TenSanPham })
            .Select(g => new DonHangViewModel
            {
                IDDH = g.Key.Iddh,
                TenKhachHang = g.Key.KhachHang,
                NgayDat = g.Key.NgayDat.HasValue ? g.Key.NgayDat.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                TenSanPham = g.Key.TenSanPham,
                TongSoLuong = g.Sum(x => x.SoLuong)
            })
            .ToList();

            return View("Index", result);
        }
    }
}

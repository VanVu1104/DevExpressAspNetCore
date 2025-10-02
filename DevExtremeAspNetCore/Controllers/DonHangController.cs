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
                        .ThenInclude(v => v.IdproNavigation)
                .ToListAsync();

            var result = donHangs.SelectMany(dh => dh.Ctdhs, (dh, ctdh) => new
            {
                dh.Iddh,
                dh.KhachHang,
                dh.NgayDat,
                TenSanPham = ctdh.IdproNavigation.TenPro,
                SoLuong = ctdh.SoLuong ?? 0
            })
            .GroupBy(x => new { x.Iddh, x.KhachHang, x.NgayDat, x.TenSanPham })
            .Select(g => new DonHangViewModel
            {
                IDDH = g.Key.Iddh,
                TenKhachHang = g.Key.KhachHang,
                NgayDat = g.Key.NgayDat,
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
                        .ThenInclude(v => v.IdproNavigation)
                .ToListAsync();

            var result = donHangs.SelectMany(dh => dh.Ctdhs, (dh, ctdh) => new
            {
                dh.Iddh,
                dh.KhachHang,
                dh.NgayDat,
                TenSanPham = ctdh.IdproNavigation.TenPro,
                SoLuong = ctdh.SoLuong ?? 0
            })
            .GroupBy(x => new { x.Iddh, x.KhachHang, x.NgayDat, x.TenSanPham })
            .Select(g => new DonHangViewModel
            {
                IDDH = g.Key.Iddh,
                TenKhachHang = g.Key.KhachHang,
                NgayDat = g.Key.NgayDat,
                TenSanPham = g.Key.TenSanPham,
                TongSoLuong = g.Sum(x => x.SoLuong)
            })
            .ToList();
            return View("Index", result);
        }

        public IActionResult Create()
        {
            return PartialView("_CreateDonHang");
        }
        [HttpPost]
        public async Task<IActionResult> Create(DonHangCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateDonHang", model);
            }

            var donHang = new DonHangModels
            {
                KhachHang = model.KhachHang,
                NgayDat = model.NgayDat
            };
            _db.DonHangs.Add(donHang);
            await _db.SaveChangesAsync();

           return RedirectToAction("Index");
        }
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateDonHang(int id, [FromBody] DonHangModels donHang)
		{
			if (id != donHang.Iddh) return BadRequest();

			_db.Entry(donHang).State = EntityState.Modified;
			await _db.SaveChangesAsync();
			return Ok(donHang);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDonHang(int id)
		{
			var donHang = await _db.DonHangs.FindAsync(id);

			if (donHang == null) return NotFound();
			_db.DonHangs.Remove(donHang);
			await _db.SaveChangesAsync();
			return Ok(new { message = "Đã xóa đơn hàng." });
		}
	}
}

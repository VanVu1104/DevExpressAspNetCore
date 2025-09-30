using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using DevExtremeAspNetCore.Repository;

namespace DevExtremeAspNetCore.Controllers
{
    public class ChiTietDonHangController : Controller
    {
        private readonly AppDbContext _db;
        private IChiTietDonHangRepository _chiTietDonHangRepository;
        public ChiTietDonHangController(AppDbContext db, IChiTietDonHangRepository chiTietDonHangRepository)
        {
            _db = db;
            _chiTietDonHangRepository = chiTietDonHangRepository;
        }

        public async Task<IActionResult> Index()
        {
            var sizes = await _chiTietDonHangRepository.GetAllSize();
            ViewBag.Sizes = sizes;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int skip = 0, int take = 10)
        {
            int pageNumber = (skip / take) + 1;
            int pageSize = take;

            var result = await _chiTietDonHangRepository.GetAllAsync(pageNumber, pageSize);

            return Json(new
            {
                data = result.Items,
                totalCount = result.TotalCount
            });
        }
    }
}

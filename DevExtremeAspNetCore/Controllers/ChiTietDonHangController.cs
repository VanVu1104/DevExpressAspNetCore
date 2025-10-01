using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using DevExtremeAspNetCore.Repository;

namespace DevExtremeAspNetCore.Controllers
{
    [Route("[controller]")]
    public class ChiTietDonHangController : Controller
    {
        private readonly AppDbContext _db;
        private IChiTietDonHangRepository _chiTietDonHangRepository;
        private IProductRepository _productRepository;
        public ChiTietDonHangController(AppDbContext db, IChiTietDonHangRepository chiTietDonHangRepository, IProductRepository productRepository)
        {
            _db = db;
            _chiTietDonHangRepository = chiTietDonHangRepository;
            _productRepository = productRepository;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var sizes = await _chiTietDonHangRepository.GetAllSize();
            ViewBag.Sizes = sizes;
            return View();
        }

        [HttpGet("GetAll")]
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
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _productRepository.GetAll();
            return Ok(products);
        }

        [HttpGet("GetColorsByProduct/{idPro}")]
        public async Task<IActionResult> GetColorsByProduct(int idPro)
        {
            var colors = await _productRepository.GetColorsByProduct(idPro);
            return Ok(colors);
        }

        [HttpGet("GetSizesByProductAndColor/{idPro}/{idColor}")]
        public async Task<IActionResult> GetSizesByProductAndColor(int idPro, int idColor)
        {
            var sizes = await _productRepository.GetSizesByProductAndColor(idPro, idColor);
            return Ok(sizes);
        }
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var products = await _productRepository.GetAll();
            ViewBag.Products = products;

            // Load size từ repo (nếu cần dùng mặc định)
            var sizes = await _chiTietDonHangRepository.GetAllSize();
            ViewBag.Sizes = sizes;

            return PartialView("_Create");
        }
        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> SaveOrderDetail([FromBody] OrderDetailCreateDto dto)
        {
            if (dto == null || dto.Sizes == null || !dto.Sizes.Any())
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });
            foreach (var s in dto.Sizes)
            {
                var ctdh = new Ctdh
                {
                    TenChiTietDonHang = dto.TenChiTietDonHang,
                    NgayGiaoHang = dto.NgayGiaoHang,
                    Iddh = dto.IdDonHang,   // mặc định 3 để test
                    Idvariant = s.IdProductVariant,
                    SoLuong = s.SoLuong
                };
                _db.Ctdhs.Add(ctdh);
            }

            await _db.SaveChangesAsync();
            return Ok(new { success = true });
        }
    }
}

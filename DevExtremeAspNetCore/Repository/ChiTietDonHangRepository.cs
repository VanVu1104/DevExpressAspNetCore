using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace DevExtremeAspNetCore.Repository
{
    public class ChiTietDonHangRepository : IChiTietDonHangRepository
    {
        private readonly AppDbContext _db;
        public ChiTietDonHangRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<ChiTietDonHangViewModel>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var donHangs = await _db.DonHangs
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdproNavigation)
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdcolorNavigation)
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.IdsizeNavigation)
                .Include(dh => dh.Ctdhs)
                    .ThenInclude(ct => ct.NoteChiTietDonHangs)
                .ToListAsync();

            var query = donHangs.SelectMany(dh => dh.Ctdhs, (dh, ctdh) => new
            {
                dh.Iddh,
                ctdh.TenChiTietDonHang,
                dh.NgayDat,
                dh.KhachHang,
                ProductName = ctdh.IdproNavigation.TenPro,
                Color = ctdh.IdcolorNavigation.TenColor,
                Size = ctdh.IdsizeNavigation.TenSize,
                SoLuong = ctdh.Soluong,
                Notes = ctdh.NoteChiTietDonHangs
                    .Select(n => new NoteChiTietDonHangViewModel
                    {
                        UrlImage = n.UrlImage,
                        UrlFile = n.UrlFile,
                        Caption = n.NoiDung
                    }).ToList()
            }).ToList();

            var grouped = query
                .GroupBy(x => new
                {
                    x.Iddh,
                    x.TenChiTietDonHang,
                    x.NgayDat,
                    x.KhachHang,
                    x.ProductName,
                    x.Color,
                })
                .Select(g => new ChiTietDonHangViewModel
                {
                    IDDH = g.Key.Iddh,
                    TenChiTietDonHang = g.Key.TenChiTietDonHang,
                    NgayDat = g.Key.NgayDat.HasValue
                        ? g.Key.NgayDat.Value.ToDateTime(TimeOnly.MinValue)
                        : DateTime.MinValue,
                    KhachHang = g.Key.KhachHang,
                    ProductName = g.Key.ProductName,
                    Color = g.Key.Color,
                    Notes = g.SelectMany(x => x.Notes).ToList(),
                    SizeQuantities = g.GroupBy(x => x.Size)
                                      .ToDictionary(sg => sg.Key, sg => sg.Sum(x => x.SoLuong ?? 0))
                })
                .Where(dh => dh.TongSoLuong > 0)
                .ToList();

            var totalCount = grouped.Count;

            var items = grouped
                .OrderBy(g => g.NgayDat)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<ChiTietDonHangViewModel>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<List<string>> GetAllSize()
        {
            var lstSize = await _db.Sizes.Select(s => s.TenSize).ToListAsync();
            return lstSize;
        }
    }
}

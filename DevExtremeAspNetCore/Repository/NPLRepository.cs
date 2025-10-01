using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DevExtremeAspNetCore.Repository
{
    public class NPLRepository:INPLRepository
    {
        private readonly AppDbContext _db;

        public NPLRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<NPLViewModel>> GetAllAsync()
        {
            var nplList = await _db.Npls
                .Include(n => n.NoteNpls) 
                .ToListAsync(); 

            var nplViewModels = nplList.Select(n => new NPLViewModel
            {
                IDNPL = n.Idnpl,
                TenNPL = n.TenNpl,
                ColorNPL = n.ColorNpl,
                KhoVai = n.KhoVai,
                Loai = n.Loai,
                DonVi = n.DonVi,
                SoLuong = n.SoLuong,

                Images = _db.ListNpls
                            .Where(l => l.Idnpl == n.Idnpl)
                            .Join(_db.ProductVariants,
                                  l => l.Idpro,
                                  v => v.Idpro,
                                  (l, v) => v.Idvariant)
                            .Join(_db.Images,
                                  vid => vid,
                                  img => img.Idvariant,
                                  (vid, img) => img.Url)
                            .ToList(),

                GhiChu = n.NoteNpls.Select(note => note.NoiDung).ToList(),
                UrlFiles = n.NoteNpls.Select(note => note.Urlfile).ToList(),
                UrlImages = n.NoteNpls.Select(note => note.Urlimage).ToList()
            }).ToList();

            return nplViewModels;
        }
    }
}

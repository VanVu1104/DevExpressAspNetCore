using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DevExtremeAspNetCore.Repository
{
    public class NPLRepository: INPLRepository
    {
        private readonly AppDbContext _db;

        public NPLRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<NPLViewModel>> GetAllAsync()
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
                                  l => l.Idvariant,

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


        public async Task AddAsync(NPLViewModel model)
        {
            var npl = new Npl
            {
                TenNpl = model.TenNPL,
                ColorNpl = model.ColorNPL,
                KhoVai = model.KhoVai,
                Loai = model.Loai,
                DonVi = model.DonVi,
                SoLuong = model.SoLuong,
                //ImagesJson = string.Join(";", model.Images ?? new List<string>())
            };
            _db.Npls.Add(npl);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(NPLViewModel model)
        {
            var npl = await _db.Npls.FindAsync(model.IDNPL);
            if (npl == null) return;

            npl.TenNpl = model.TenNPL;
            npl.ColorNpl = model.ColorNPL;
            npl.KhoVai = model.KhoVai;
            npl.Loai = model.Loai;
            npl.DonVi = model.DonVi;
            npl.SoLuong = model.SoLuong;
            //npl.Ur = string.Join(";", model.Images ?? new List<string>());

            _db.Npls.Update(npl);
            await _db.SaveChangesAsync();
        }

        public async Task<NPLViewModel> GetByIdAsync(string id)
        {
            if (!int.TryParse(id, out int nplId))
                return null;

            var npl = await _db.Npls.FindAsync(nplId);
            if (npl == null) return null;

            return new NPLViewModel
            {
                IDNPL = npl.Idnpl,
                TenNPL = npl.TenNpl,
                ColorNPL = npl.ColorNpl,
                KhoVai = npl.KhoVai,
                Loai = npl.Loai,
                DonVi = npl.DonVi,
                SoLuong = npl.SoLuong
                //Images = ...
            };
        }

        public async Task DeleteAsync(string id)
        {
            if (!int.TryParse(id, out int nplId))
                return;

            var npl = await _db.Npls.FindAsync(nplId);
            if (npl == null) return;

            _db.Npls.Remove(npl);
            await _db.SaveChangesAsync();
        }

        public async Task<List<string>> GetColorsAsync(string term = "")
        {
            return await _db.Npls
                .Where(x => !string.IsNullOrEmpty(x.ColorNpl) && x.ColorNpl.Contains(term))
                .Select(x => x.ColorNpl)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetUnitsAsync()
        {
            return await _db.Npls
                .Where(x => !string.IsNullOrEmpty(x.DonVi))
                .Select(x => x.DonVi)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetTypesAsync()
        {
            return await _db.Npls
                .Where(x => !string.IsNullOrEmpty(x.Loai))
                .Select(x => x.Loai)
                .Distinct()
                .ToListAsync();
        }
    }
}

using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace DevExtremeAspNetCore.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductDTO>> GetAll()
        {
            return await _context.Products
                .Select(p => new ProductDTO
                {
                    Id = p.Idpro,
                    Name = p.TenPro
                })
                .ToListAsync();
        }

        public async Task<List<ColorDto>> GetColorsByProduct(int idPro)
        {
            return await _context.ProductVariants
                .Where(v => v.Idpro == idPro)
                .Select(v => new ColorDto
                {
                    Id = v.Idcolor,
                    Name = v.IdcolorNavigation.TenColor
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<SizeDto>> GetSizesByProductAndColor(int idPro, int idColor)
        {
            return await _context.ProductVariants
                .Where(v => v.Idpro == idPro && v.Idcolor == idColor)
                .Select(v => new SizeDto
                {
                    VariantId = v.Idvariant,
                    Name = v.IdsizeNavigation.TenSize
                })
                .ToListAsync();
        }
    }
}

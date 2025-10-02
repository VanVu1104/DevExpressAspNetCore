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
        public async Task<List<ProductDTO>> GetProducts()
        {
            return await _context.Products
                .Select(p => new ProductDTO
                {
                    Id = p.Idpro,
                    Name = p.TenPro
                })
                .ToListAsync();
        }

        public async Task<List<ColorDto>> GetColors()
        {
            return await _context.Colors
                .Select(v => new ColorDto
                {
                    Id = v.Idcolor,
                    Name = v.TenColor
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<SizeDto>> GetSizes()
        {
            return await _context.Sizes
                .Select(v => new SizeDto
                {
                    Id = v.Idsize,
                    Name = v.TenSize
                })
                .ToListAsync();
        }
    }
}

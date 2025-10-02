using DevExtremeAspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevExtremeAspNetCore.ViewComponents
{
    public class ProductSelectViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductSelectViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int? selectedId = null)
        {
            var products = _context.Products
                .Select(p => new SelectListItem
                {
                    Value = p.Idpro.ToString(),
                    Text = p.TenPro
                })
                .ToList();

            ViewBag.Products = products;

            return View(selectedId);
        }
    }
}

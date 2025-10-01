using DevExtremeAspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevExtremeAspNetCore.ViewComponents
{
    public class SizeSelectViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public SizeSelectViewComponent(AppDbContext context)
        {
            _context = context;
        }

        // size phụ thuộc vào product + color
        public IViewComponentResult Invoke(int idProduct, int idColor, int? selectedId = null)
        {
            var sizes = _context.ProductVariants
                .Where(v => v.Idpro == idProduct && v.Idcolor == idColor)
                .Select(v => new SelectListItem
                {
                    Value = v.IdsizeNavigation.Idsize.ToString(),
                    Text = v.IdsizeNavigation.TenSize
                })
                .Distinct()
                .ToList();

            ViewBag.Sizes = sizes;

            return View(selectedId);
        }
    }
}

using DevExtremeAspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevExtremeAspNetCore.ViewComponents
{
    public class ColorSelectViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ColorSelectViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int? selectedId = null)
        {
            var colors = _context.Colors
                .Select(p => new SelectListItem
                {
                    Value = p.Idcolor.ToString(),
                    Text = p.TenColor
                })
                .ToList();

            ViewBag.Colors = colors;

            return View(selectedId);
        }
    }

}

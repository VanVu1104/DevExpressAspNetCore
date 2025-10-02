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
        //Load danh sách size
        public IViewComponentResult Invoke(int? selectedId = null)
        {
            var sizes = _context.Sizes
                .Select(p => new SelectListItem
                {
                    Value = p.Idsize.ToString(),
                    Text = p.TenSize
                })
                .ToList();

            ViewBag.Sizes = sizes;

            return View(selectedId);
        }
    }
}

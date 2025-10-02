using DevExtremeAspNetCore.Repository;
using DevExtremeAspNetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevExtremeAspNetCore.Controllers
{
    public class NPLController : Controller
    {
        private readonly INPLRepository _nplRepo;

        public NPLController(INPLRepository nplRepo)
        {
            _nplRepo = nplRepo;
        }

        public async Task<IActionResult> Index()
        {
            var nplList = await _nplRepo.GetAllAsync();
            return View(nplList);
        }
        [HttpPost]
        public async Task<IActionResult> Create(NPLViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _nplRepo.AddAsync(model);
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NPLViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _nplRepo.UpdateAsync(model);
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _nplRepo.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<JsonResult> GetColors(string term)
        {
            var data = await _nplRepo.GetColorsAsync(term);
            return Json(data.Select(x => new { Name = x }));
        }
    }

}

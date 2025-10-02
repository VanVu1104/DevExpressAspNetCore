using DevExtremeAspNetCore.Repository;
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
    }

}

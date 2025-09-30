using DevExtremeAspNetCore.Services;
using DevExtremeAspNetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevExtremeAspNetCore.Controllers
{
    [Route("Note")]
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("Index")]
        public IActionResult Upload()
        {
            return View("Index");
        }
        [HttpPost("Index")]
        public async Task<IActionResult> Upload(List<NoteUploadViewModel> items)
        {
            // test: set cứng IDCTDH = 1
            int idCTDH = 1;

            if (items == null || items.Count == 0)
            {
                ViewBag.Message = "Vui lòng chọn file hoặc ảnh.";
                return View();
            }

            var urls = await _noteService.UploadNotesAsync(items, idCTDH);
            foreach (var u in urls)
            {
                Console.WriteLine("Uploaded URL: " + u);
            }

            ViewBag.Message = "Upload thành công!";
            ViewBag.Urls = urls;

            return View("Index");
        }
    }
}

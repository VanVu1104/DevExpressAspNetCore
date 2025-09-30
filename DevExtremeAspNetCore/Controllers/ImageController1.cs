using DevExtremeAspNetCore.Service;
using DevExtremeAspNetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevExtremeAspNetCore.Controllers
{
    [Route("Image")]
    public class ImageController1 : Controller
    {
        private readonly IImageService _imageService;

        public ImageController1(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("Upload")]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(List<ImageUploadViewModel> items)
        {
            if (items == null || items.Count == 0)
            {
                ViewBag.Message = "Vui lòng chọn ít nhất một hình và nội dung.";
                return View();
            }

            var urls = await _imageService.UploadAndSaveImagesAsync(items, 1);

            ViewBag.Message = "Upload thành công!";
            ViewBag.Urls = urls;

            return View();
        }
    }
}

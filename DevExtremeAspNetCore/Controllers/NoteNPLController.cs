using DevExtremeAspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevExtremeAspNetCore.Controllers
{
    public class NoteNPLController : Controller
    {
        private readonly NoteNPLRepository nplrepo;

        public NoteNPLController(NoteNPLRepository repo)
        {
            nplrepo = repo;
        }
        //[HttpGet("Upload")]
        //public IActionResult Upload()
        //{
        //    return View();
        //}
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAndImage(
            [FromForm] int idNPL,
            [FromForm] IFormFile file,          // file tài liệu (pdf, doc, xlsx…)
            [FromForm] IFormFile image,         // file ảnh (png, jpg…)
            [FromForm] string noiDung)          // nội dung mô tả
        {
            if (file == null && image == null)
                return BadRequest("Phải chọn ít nhất 1 file hoặc ảnh.");

            string urlFile = null;
            string urlImage = null;

            // Upload file tài liệu
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine("wwwroot/uploads/files", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                urlFile = "/uploads/files/" + fileName;
            }

            // Upload file ảnh
            if (image != null && image.Length > 0)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine("wwwroot/uploads/images", imageName);

                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                urlImage = "/uploads/images/" + imageName;
            }

            // Lưu vào DB
            var note = new NoteNpl
            {
                Idnpl = idNPL,
                Urlfile = urlFile,
                Urlimage = urlImage,
                NoiDung = noiDung
            };

            await nplrepo.AddAsync(note);

            return Ok(new
            {
                note.IdnoteNpl,
                note.Idnpl,
                note.Urlfile,
                note.Urlimage,
                note.NoiDung
            });
        }
    }
}

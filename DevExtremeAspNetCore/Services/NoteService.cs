using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using DXWebApplication4.Services;

namespace DevExtremeAspNetCore.Services
{
    public class NoteService : INoteService   // 👈 implements interface
    {
        private readonly FirebaseService _firebaseService;
        private readonly AppDbContext _context;

        public NoteService(FirebaseService firebaseService, AppDbContext context)
        {
            _firebaseService = firebaseService;
            _context = context;
        }

        public async Task<List<string>> UploadNotesAsync(List<NoteUploadViewModel> items, int idCTDH)
        {
            var uploadedUrls = new List<string>();

            foreach (var item in items)
            {
                if (item.File != null && item.File.Length > 0)
                {
                    using (var stream = item.File.OpenReadStream())
                    {
                        string url;

                        if (item.IsImage)
                            url = await _firebaseService.UploadFileAsync(stream, item.File.FileName, "images");
                        else
                            url = await _firebaseService.UploadFileNoteAsync(stream, item.File.FileName, "notes");

                        // Lưu DB
                        var note = new NoteChiTietDonHang
                        {
                            Idctdh = idCTDH,
                            UrlFile = item.IsImage ? null : url,
                            UrlImage = item.IsImage ? url : null,
                            NoiDung = item.NoiDung
                        };

                        _context.NoteChiTietDonHangs.Add(note);
                        uploadedUrls.Add(url);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return uploadedUrls;
        }
    }
}

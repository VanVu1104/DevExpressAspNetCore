using DevExtremeAspNetCore.Models;
using DevExtremeAspNetCore.ViewModels;
using DXWebApplication4.Services;

namespace DevExtremeAspNetCore.Service
{
    public class ImageService : IImageService  
    {
        private readonly FirebaseService _firebaseService;
        private readonly AppDbContext _context;

        public ImageService(FirebaseService firebaseService, AppDbContext context)
        {
            _firebaseService = firebaseService;
            _context = context;
        }

        public async Task<List<string>> UploadAndSaveImagesAsync(List<ImageUploadViewModel> items, int idVariant)
        {
            var uploadedUrls = new List<string>();

            foreach (var item in items)
            {
                if (item.File != null && item.File.Length > 0)
                {
                    using (var stream = item.File.OpenReadStream())
                    {
                        var url = await _firebaseService.UploadFileAsync(stream, item.File.FileName, "images");

                        var img = new Image
                        {
                            Idvariant = idVariant,
                            Url = url,
                            NoiDung = item.NoiDung
                        };

                        _context.Images.Add(img);
                        uploadedUrls.Add(url);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return uploadedUrls;
        }
    }
}

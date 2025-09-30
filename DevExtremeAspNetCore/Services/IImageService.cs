using DevExtremeAspNetCore.ViewModels;

namespace DevExtremeAspNetCore.Service
{
    public interface IImageService
    {
        Task<List<string>> UploadAndSaveImagesAsync(List<ImageUploadViewModel> items, int idVariant);
    }
}

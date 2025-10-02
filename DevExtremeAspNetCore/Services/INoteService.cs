using DevExtremeAspNetCore.ViewModels;

namespace DevExtremeAspNetCore.Services
{
    public interface INoteService
    {
        Task<List<string>> UploadNotesAsync(List<NoteUploadViewModel> items, int idCTDH);
    }
}

using System.ComponentModel.DataAnnotations;

namespace DevExtremeAspNetCore.ViewModels
{
    public class NoteUploadViewModel
    {
        [Required]
        public IFormFile File { get; set; }   

        public string NoiDung { get; set; }  

        public bool IsImage { get; set; }   
    }
}

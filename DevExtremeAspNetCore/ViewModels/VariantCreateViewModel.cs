namespace DevExtremeAspNetCore.ViewModels
{
    public class VariantCreateViewModel
    {
        public int IDSize { get; set; }
        public int IDColor { get; set; }

        public List<IFormFile> Images { get; set; }
        public List<string> Notes { get; set; }
        public List<NPLCreateViewModel> NPLs { get; set; }
    }
}

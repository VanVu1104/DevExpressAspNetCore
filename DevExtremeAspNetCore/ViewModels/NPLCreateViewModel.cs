namespace DevExtremeAspNetCore.ViewModels
{
    public class NPLCreateViewModel
    {
        public int? IDNPL { get; set; } 
        public string TenNPL { get; set; }
        public string ColorNPL { get; set; }
        public int? KhoVai { get; set; }
        public string Loai { get; set; }
        public string DonVi { get; set; }
        public int SoLuong { get; set; }

        public List<IFormFile> Files { get; set; } 
        public List<IFormFile> Images { get; set; } 
        public string NoiDung { get; set; }
    }
}

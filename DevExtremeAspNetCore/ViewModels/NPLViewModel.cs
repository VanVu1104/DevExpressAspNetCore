namespace DevExtremeAspNetCore.ViewModels
{
    public class NPLViewModel
    {
        public int IDNPL { get; set; }
        public string TenNPL { get; set; }
        public string ColorNPL { get; set; }
        public int? KhoVai { get; set; }
        public string Loai { get; set; }
        public string DonVi { get; set; }
        public int? SoLuong { get; set; }

        public List<string> Images { get; set; } = new List<string>();
        public string ImagesInput { get; set; }
        public List<string> UrlFiles { get; set; } = new List<string>();
        public List<string> UrlImages { get; set; } = new List<string>();
        public List<string> GhiChu { get; set; } = new List<string>();
    }
}

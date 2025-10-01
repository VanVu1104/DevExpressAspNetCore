namespace DevExtremeAspNetCore.ViewModels
{
    public class SizeQuantityDto
    {
        public int IdProductVariant { get; set; }
        public int SoLuong { get; set; }
    }

    public class OrderDetailCreateDto
    {
        public int IdDonHang { get; set; } 
        public string TenChiTietDonHang { get; set; }
        public DateTime NgayGiaoHang { get; set; }
        public string KhachHang { get; set; }
        public int IdProduct { get; set; }
        public int IdColor { get; set; }
        public List<SizeQuantityDto> Sizes { get; set; }
    }
}

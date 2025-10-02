using System.ComponentModel.DataAnnotations;

namespace DevExtremeAspNetCore.ViewModels
{
    public class ChiTietDonHangCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đơn hàng")]
        [Display(Name = "Tên đơn hàng")]
        public string TenChiTietDonHang { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày đặt")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày đặt")]
        public DateTime NgayGiaoHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [Display(Name = "Khách hàng")]
        public string KhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public int IdProduct { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn màu sắc")]
        [Display(Name = "Màu sắc")]
        public int IdColor { get; set; }
        public Dictionary<int, int> SizeQuantities { get; set; } = new();

        // Ảnh và file tạm thời bạn để text placeholder nên chưa cần IFormFile,
        // nhưng nếu sau này có upload file thì thêm vào như sau:
        // public IFormFile? ImageFile { get; set; }
        // public IFormFile? Attachment { get; set; }
    }
}

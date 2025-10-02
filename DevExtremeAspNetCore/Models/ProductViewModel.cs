using System.ComponentModel.DataAnnotations;
// XÓA: using System.Web; (Nếu nó chỉ dùng cho HttpPostedFileBase)
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; // Cần thêm namespace này cho IFormFile

namespace DevExtremeAspNetCore.Models
{
    public class ProductViewModel
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Kích thước là bắt buộc.")]
        public string ProductSize { get; set; }

        [Required(ErrorMessage = "Màu sắc là bắt buộc.")]
        public string ProductColor { get; set; }

        // SỬA: Thay thế HttpPostedFileBase bằng IFormFile
        public IEnumerable<IFormFile> TestImages { get; set; }
        public IEnumerable<IFormFile> TestDocuments { get; set; }
    }
}
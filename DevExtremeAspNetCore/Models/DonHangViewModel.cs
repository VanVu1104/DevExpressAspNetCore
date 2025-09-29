using System;
using System.Collections.Generic;

namespace DevExtremeAspNetCore.ViewModels
{
    // Model ảnh cho đơn hàng
    public class DonHangImage
    {
        public string Url { get; set; }
        public string Caption { get; set; }
    }

    // Model file đính kèm cho đơn hàng
    public class DonHangFile
    {
        public string Url { get; set; }
        public string FileName { get; set; }
    }

    public class DonHangViewModel
    {
        public int IDDH { get; set; }
        public DateTime NgayDat { get; set; }
        public string KhachHang { get; set; }

        // Thông tin sản phẩm trong đơn hàng
        public string ProductName { get; set; }
        public string Color { get; set; }

        // Số lượng theo size
        public int XXS { get; set; }
        public int XSM { get; set; }
        public int SM { get; set; }
        public int MED { get; set; }
        public int LRG { get; set; }
        public int XLG { get; set; }
        public int XXL { get; set; }

        public int TongSoLuong =>
            XXS + XSM + SM + MED + LRG + XLG + XXL;

        // Danh sách ảnh đơn hàng (mapping từ bảng Image)
        public List<DonHangImage> Images { get; set; } = new List<DonHangImage>();

        // Danh sách file đính kèm đơn hàng (mapping từ bảng FileDonHang)
        public List<DonHangFile> Files { get; set; } = new List<DonHangFile>();
    }
}

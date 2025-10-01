using System;
using System.Collections.Generic;

namespace DevExtremeAspNetCore.ViewModels
{
    public class ChiTietDonHangViewModel
    {
        public int IDCTDH { get; set; }
        public int IDDH { get; set; }
        public string TenChiTietDonHang { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime NgayGiaoHang { get; set; }
        public string KhachHang { get; set; }

        // Thông tin sản phẩm trong đơn hàng
        public string ProductName { get; set; }
        public string Color { get; set; }
        public Dictionary<string, int> SizeQuantities { get; set; } = new();
        public List<string> AllSizes { get; set; } = new();
        public int TongSoLuong => SizeQuantities.Values.Sum();
        public string ImageUrl => Notes?.FirstOrDefault(n => !string.IsNullOrEmpty(n.UrlImage))?.UrlImage;
        public string FilePath => Notes?.FirstOrDefault(n => !string.IsNullOrEmpty(n.UrlFile))?.UrlFile;

        public List<NoteChiTietDonHangViewModel> Notes { get; set; } = new();

    }
    public class NoteChiTietDonHangViewModel
    {
        public string UrlImage { get; set; }
        public string UrlFile { get; set; }
        public string Caption { get; set; }
    }
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }
}

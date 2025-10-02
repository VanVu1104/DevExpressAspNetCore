using System;
using System.Collections.Generic;

namespace DevExtremeAspNetCore.ViewModels
{
    public class DonHangViewModel
    {
        public int IDDH { get; set; }  
        public string TenKhachHang { get; set; }
        public DateTime NgayDat { get; set; } 
        public string TenSanPham { get; set; }
        public int TongSoLuong { get; set; }
        //public List<ChiTietDonHangViewModel> Ctdhs { get; set; } = new();
    }
}

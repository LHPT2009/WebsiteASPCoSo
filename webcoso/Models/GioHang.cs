using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace webcoso.Models
{
    public class GioHang
    {
        WebcosoContext data = new WebcosoContext();
        [Display(Name = "Mã sản phẩm")]
        public int MaSP { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Ten { get; set; }

        [Display(Name = "Ảnh")]
        public string Hinh { get; set; }

        [Display(Name = "Giá bán")]
        public double Gia { get; set; }

        [Display(Name = "Giảm giá")]
        public double GiamGia { get; set; }

        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Thành tiền")]
        public double ThanhTien
        {
            get { return SoLuong * (Gia - GiamGia); }
        }

        public GioHang(int id)
        {
            MaSP = id;
            SanPham sp = data.SanPham.Single(n => n.MaSP == MaSP);
            Ten = sp.Ten;
            Hinh = sp.Hinh;
            Gia = double.Parse(sp.Gia.ToString());
            SoLuong = 1;
            GiamGia = 0;
        }
        public GioHang()
        {
            MaSP = 1;
            Ten = "Bo";
            Hinh = "";
            Gia = 1000;
            SoLuong = 2;
            GiamGia = 3;
        }
    }
}
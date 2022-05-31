using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webcoso.Models
{
    public class SanPhamDetailModel
    {
        public SanPham SanPham { get; set; }
        public PagedList.PagedList<BinhLuan> BinhLuans { get; set; }
        public PagedList.PagedList<DanhGia> danhGias { get; set; }
    }
}
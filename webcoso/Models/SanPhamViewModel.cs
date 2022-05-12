using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webcoso.Models
{
    public class SanPhamViewModel
    {
        public IEnumerable<LoaiSP> LoaiSPs { get; set; }
        public PagedList.PagedList<SanPham> SanPhams { get; set; }
    }
}
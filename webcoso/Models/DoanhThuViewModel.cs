using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webcoso.Models
{
    public class DoanhThuViewModel
    {
        public PagedList.PagedList<DateTime> ThangNam { get; set; }
        public List<Double> Result { get; set; }
        public double Total { get; set; }
    }
}
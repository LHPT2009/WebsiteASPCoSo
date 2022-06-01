namespace webcoso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhGia")]
    public partial class DanhGia
    {
        [Key]
        public int MaDG { get; set; }

        public int? SoSao { get; set; }

        public DateTime? NgayTao { get; set; }

        public int? MaSP { get; set; }

        [StringLength(128)]
        public string MaKH { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
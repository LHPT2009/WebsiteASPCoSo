namespace webcoso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("DonHang")]
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            ChiTietDonHang = new HashSet<ChiTietDonHang>();
        }

        [Key]
        public int MaDH { get; set; }

        public byte? TrangThaiGiaoHang { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayDat { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayGiao { get; set; }

        [Required]
        [StringLength(128)]
        public string MaKH { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public double? TongTien { get; set; }

        public bool? TrangThaiThanhToan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; }

        //  public virtual Models.LinQ.AspNetUser User { get; set; }

        public static List<DonHang> getAll(String keyWord)
        {
            WebcosoContext db = new WebcosoContext();
            keyWord = keyWord + " ";
            //List<ChiTietDonHang> list = db.ChiTietDonHang.Where(a => a.SanPham.Ten.Contains(keyWord) && a.Soluong > 0).ToList();
            return db.DonHang.ToList();
        }

    }
}

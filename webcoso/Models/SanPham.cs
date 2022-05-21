namespace webcoso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            BinhLuan = new HashSet<BinhLuan>();
            ChiTietDonHang = new HashSet<ChiTietDonHang>();
        }

        [Key]
        public int MaSP { get; set; }

        public int MaLoai { get; set; }

        [StringLength(255)]
        public string Ten { get; set; }

        [StringLength(255)]
        public string MoTa { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public double? Gia { get; set; }

        public int? SoLuong { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public double? GiamGia { get; set; }

        [StringLength(255)]
        public string Hinh { get; set; }

        [StringLength(128)]
        public string CPU { get; set; }

        [StringLength(128)]
        public string ManHinh { get; set; }

        [StringLength(128)]
        public string RAM { get; set; }

        [StringLength(128)]
        public string OCung { get; set; }

        [StringLength(128)]
        public string VGA { get; set; }

        [StringLength(128)]
        public string HDH { get; set; }

        [StringLength(64)]
        public string Mau { get; set; }

        public int? BaoHang { get; set; }

        public int? MaTH { get; set; }

        public int? MaNhom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuan> BinhLuan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; }

        public virtual LoaiSP LoaiSP { get; set; }

        public virtual NhomSanPham NhomSanPham { get; set; }

        public virtual ThuongHieu ThuongHieu { get; set; }

        public static List<SanPham> getAll(string searchKey)
        {
            WebcosoContext db = new WebcosoContext();
            searchKey = searchKey + "";
            return db.SanPham.Where(p => p.Ten.Contains(searchKey) && p.SoLuong > 0).ToList();
        }
        public static List<SanPham> getAllgiaduoi2()
        {
            WebcosoContext db = new WebcosoContext();
            return db.SanPham.Where(p => p.Gia < 20000000 && p.SoLuong > 0).ToList();
        }
        public static List<SanPham> getAllgia23()
        {
            WebcosoContext db = new WebcosoContext();
            return db.SanPham.Where(p => p.Gia >= 20000000 && p.Gia < 35000000 && p.SoLuong > 0).ToList();
        }
        public static List<SanPham> getAllgiatren3()
        {
            WebcosoContext db = new WebcosoContext();
            return db.SanPham.Where(p => p.Gia >= 35000000 && p.SoLuong > 0).ToList();
        }
        public static List<SanPham> getsanphamtheoid(int maloai)
        {
            WebcosoContext db = new WebcosoContext();
            return db.SanPham.Where(p => p.MaLoai == maloai && p.SoLuong > 0).ToList();
        }
    }
}

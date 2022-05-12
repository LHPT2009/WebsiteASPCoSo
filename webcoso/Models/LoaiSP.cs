namespace webcoso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("LoaiSP")]
    public partial class LoaiSP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiSP()
        {
            SanPham = new HashSet<SanPham>();
        }

        [Key]
        public int MaLoai { get; set; }

        [StringLength(30)]
        public string TenLoai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPham { get; set; }

        public static List<LoaiSP> getAll(string searchKey)
        {
            WebcosoContext db = new WebcosoContext();
            searchKey = searchKey + "";
            return db.LoaiSP.Where(p => p.TenLoai.Contains(searchKey)).ToList();
        }
    }
}

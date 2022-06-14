namespace webcoso.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LienHe")]
    public partial class LienHe
    {
        [Key]
        public int IdLienHe { get; set; }

        public DateTime? NgayGui { get; set; }

        [StringLength(255)]
        public string NoiDung { get; set; }

        [Required]
        [StringLength(128)]
        public string IdUser { get; set; }
    }
}

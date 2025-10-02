using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("CTDH")]
public partial class Ctdh
{
    [Key]
    [Column("IDCTDH")]
    public int Idctdh { get; set; }

    [StringLength(250)]
    public string TenChiTietDonHang { get; set; }

    [Column("IDDH")]
    public int Iddh { get; set; }

    public int? SoLuong { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime NgayGiaoHang { get; set; }

    [Column("IDPro")]
    public int? Idpro { get; set; }

    [Column("IDColor")]
    public int? Idcolor { get; set; }

    [Column("IDSize")]
    public int? Idsize { get; set; }

    [ForeignKey("Idcolor")]
    [InverseProperty("Ctdhs")]
    public virtual Color IdcolorNavigation { get; set; }

    [ForeignKey("Iddh")]
    [InverseProperty("Ctdhs")]
    public virtual DonHangModels IddhNavigation { get; set; }

    [ForeignKey("Idpro")]
    [InverseProperty("Ctdhs")]
    public virtual ProductModels IdproNavigation { get; set; }

    [ForeignKey("Idsize")]
    [InverseProperty("Ctdhs")]
    public virtual Size IdsizeNavigation { get; set; }


    [InverseProperty("IdctdhNavigation")]
    public virtual ICollection<NoteChiTietDonHang> NoteChiTietDonHangs { get; set; } = new List<NoteChiTietDonHang>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExtremeAspNetCore.ViewModels;
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

    [Column("IDPro")]
    public int Idpro { get; set; }

    [Column("IDSize")]
    public int Idsize { get; set; }

    [Column("IDColor")]
    public int Idcolor { get; set; }

    public int? Soluong { get; set; }

    [ForeignKey("Iddh")]
    [InverseProperty("Ctdhs")]
    public virtual DonHangViewModel IddhNavigation { get; set; }

    [ForeignKey("Idpro")]
    public virtual Product IdproNavigation { get; set; }

    [ForeignKey("Idsize")]
    public virtual Size IdsizeNavigation { get; set; }

    [ForeignKey("Idcolor")]
    public virtual Color IdcolorNavigation { get; set; }

    [InverseProperty("IdctdhNavigation")]
    public virtual ICollection<NoteChiTietDonHang> NoteChiTietDonHangs { get; set; } = new List<NoteChiTietDonHang>();
}

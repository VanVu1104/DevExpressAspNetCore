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

    [Column("IDVariant")]
    public int Idvariant { get; set; }

    public int? SoLuong { get; set; }

    [ForeignKey("Iddh")]
    [InverseProperty("Ctdhs")]
    public virtual DonHang IddhNavigation { get; set; }

    [ForeignKey("Idvariant")]
    [InverseProperty("Ctdhs")]
    public virtual ProductVariant IdvariantNavigation { get; set; }


    [InverseProperty("IdctdhNavigation")]
    public virtual ICollection<NoteChiTietDonHang> NoteChiTietDonHangs { get; set; } = new List<NoteChiTietDonHang>();
}

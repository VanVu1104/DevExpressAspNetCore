using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DevExtremeAspNetCore.Models;

namespace DevExtremeAspNetCore.ViewModels;

[Table("DonHang")]
public partial class DonHang
{
    [Key]
    [Column("IDDH")]
    public int Iddh { get; set; }

    public DateOnly? NgayDat { get; set; }

    [StringLength(255)]
    public string KhachHang { get; set; }

    [InverseProperty("IddhNavigation")]
    public virtual ICollection<Ctdh> Ctdhs { get; set; } = new List<Ctdh>();

    [InverseProperty("IddhNavigation")]
    public virtual ICollection<NoteDonHang> NoteDonHangs { get; set; } = new List<NoteDonHang>();
}

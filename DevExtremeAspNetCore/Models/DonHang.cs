using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("DonHang")]
public partial class DonHangModels

{
    [Key]
    [Column("IDDH")]
    public int Iddh { get; set; }

    public DateOnly? NgayDat { get; set; }

    [StringLength(255)]
    public string KhachHang { get; set; }

    [InverseProperty("IddhNavigation")]
    public virtual ICollection<Ctdh> Ctdhs { get; set; } = new List<Ctdh>();

}

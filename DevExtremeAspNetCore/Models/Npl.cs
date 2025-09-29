using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("NPL")]
public partial class Npl
{
    [Key]
    [Column("IDNPL")]
    public int Idnpl { get; set; }

    [Required]
    [Column("TenNPL")]
    [StringLength(255)]
    public string TenNpl { get; set; }

    [Column("ColorNPL")]
    [StringLength(50)]
    public string ColorNpl { get; set; }

    public int? KhoVai { get; set; }

    [StringLength(50)]
    public string Loai { get; set; }

    [StringLength(20)]
    public string DonVi { get; set; }

    public int? SoLuong { get; set; }

    [InverseProperty("IdnplNavigation")]
    public virtual ICollection<ListNpl> ListNpls { get; set; } = new List<ListNpl>();

    [InverseProperty("IdnplNavigation")]
    public virtual ICollection<NoteNpl> NoteNpls { get; set; } = new List<NoteNpl>();
}

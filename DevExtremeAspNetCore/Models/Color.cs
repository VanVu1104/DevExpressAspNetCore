using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("Color")]
public partial class Color
{
    [Key]
    [Column("IDColor")]
    public int Idcolor { get; set; }

    [StringLength(50)]
    public string TenColor { get; set; }

    [InverseProperty("IdcolorNavigation")]
    public virtual ICollection<Ctdh> Ctdhs { get; set; } = new List<Ctdh>();

    [InverseProperty("IdcolorNavigation")]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}

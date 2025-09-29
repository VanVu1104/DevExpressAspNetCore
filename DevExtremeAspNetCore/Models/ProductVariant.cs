using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("ProductVariant")]
public partial class ProductVariant
{
    [Key]
    [Column("IDVariant")]
    public int Idvariant { get; set; }

    [Column("IDPro")]
    public int Idpro { get; set; }

    [Column("IDSize")]
    public int Idsize { get; set; }

    [Column("IDColor")]
    public int Idcolor { get; set; }

    [InverseProperty("IdvariantNavigation")]
    public virtual ICollection<Ctdh> Ctdhs { get; set; } = new List<Ctdh>();

    [ForeignKey("Idcolor")]
    [InverseProperty("ProductVariants")]
    public virtual Color IdcolorNavigation { get; set; }

    [ForeignKey("Idpro")]
    [InverseProperty("ProductVariants")]
    public virtual Product IdproNavigation { get; set; }

    [ForeignKey("Idsize")]
    [InverseProperty("ProductVariants")]
    public virtual Size IdsizeNavigation { get; set; }

    [InverseProperty("IdvariantNavigation")]
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}

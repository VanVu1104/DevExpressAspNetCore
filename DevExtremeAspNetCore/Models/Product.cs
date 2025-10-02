using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("Product")]
public partial class Product
{
    [Key]
    [Column("IDPro")]
    public int Idpro { get; set; }

    [StringLength(255)]
    public string TenPro { get; set; }

    //[InverseProperty("IdproNavigation")]
    //public virtual ICollection<ListNpl> ListNpls { get; set; } = new List<ListNpl>();
    [InverseProperty("IdcolorNavigation")]
    public virtual ICollection<Ctdh> Ctdhs { get; set; } = new List<Ctdh>();

    [InverseProperty("IdproNavigation")]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}

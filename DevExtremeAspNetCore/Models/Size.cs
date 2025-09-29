using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("Size")]
public partial class Size
{
    [Key]
    [Column("IDSize")]
    public int Idsize { get; set; }

    [StringLength(50)]
    public string TenSize { get; set; }

    [InverseProperty("IdsizeNavigation")]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}

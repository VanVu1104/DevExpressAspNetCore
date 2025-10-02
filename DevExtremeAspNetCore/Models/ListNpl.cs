using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("ListNPL")]
public partial class ListNpl
{
    [Key]
    [Column("IDList")]
    public int Idlist { get; set; }

    [Column("IDVariant")]
    public int Idvariant { get; set; }

    [Column("IDNPL")]
    public int Idnpl { get; set; }

    [ForeignKey("Idnpl")]
    [InverseProperty("ListNpls")]
    public virtual Npl IdnplNavigation { get; set; }

    [ForeignKey("Idvariant")]
    [InverseProperty("ListNpls")]
    public virtual ProductVariant IdvariantNavigation { get; set; }
}


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

    [Column("IDPro")]
    public int Idpro { get; set; }

    [Column("IDNPL")]
    public int Idnpl { get; set; }

    [ForeignKey("Idnpl")]
    [InverseProperty("ListNpls")]
    public virtual Npl IdnplNavigation { get; set; }

    [ForeignKey("Idpro")]
    [InverseProperty("ListNpls")]
    public virtual ProductModels IdproNavigation { get; set; }
}

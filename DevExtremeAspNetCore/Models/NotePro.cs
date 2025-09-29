using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("NotePro")]
public partial class NotePro
{
    [Key]
    [Column("IDNote")]
    public int Idnote { get; set; }

    [Column("IDPro")]
    public int Idpro { get; set; }

    [Column("URLFile")]
    [StringLength(500)]
    public string Urlfile { get; set; }

    [StringLength(500)]
    public string NoiDung { get; set; }

    [ForeignKey("Idpro")]
    [InverseProperty("NotePros")]
    public virtual Product IdproNavigation { get; set; }
}

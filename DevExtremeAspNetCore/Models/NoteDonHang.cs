using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("NoteDonHang")]
public partial class NoteDonHang
{
    [Key]
    [Column("IDNote")]
    public int Idnote { get; set; }

    [Column("IDDH")]
    public int Iddh { get; set; }

    [Column("URLFile")]
    [StringLength(500)]
    public string Urlfile { get; set; }

    [Column("URLImage")]
    [StringLength(500)]
    public string Urlimage { get; set; }

    [StringLength(500)]
    public string NoiDung { get; set; }

    [ForeignKey("Iddh")]
    [InverseProperty("NoteDonHangs")]
    public virtual DonHang IddhNavigation { get; set; }
}

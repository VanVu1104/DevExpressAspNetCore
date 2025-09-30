using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("NoteChiTietDonHang")]
public partial class NoteChiTietDonHang
{
    [Key]
    [Column("IDNote")]
    public int Idnote { get; set; }

    [Column("IDCTDH")]
    public int Idctdh { get; set; }

    [StringLength(500)]
    public string UrlFile { get; set; }

    [StringLength(500)]
    public string UrlImage { get; set; }

    [StringLength(500)]
    public string NoiDung { get; set; }

  
    [ForeignKey("Idctdh")]
    [InverseProperty("NoteChiTietDonHangs")]
    public virtual Ctdh IdctdhNavigation { get; set; }

}

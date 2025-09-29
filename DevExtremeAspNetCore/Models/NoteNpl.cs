using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("NoteNPL")]
public partial class NoteNpl
{
    [Key]
    [Column("IDNoteNPL")]
    public int IdnoteNpl { get; set; }

    [Column("IDNPL")]
    public int Idnpl { get; set; }

    [Column("URLFile")]
    [StringLength(500)]
    public string Urlfile { get; set; }

    [Column("URLImage")]
    [StringLength(500)]
    public string Urlimage { get; set; }

    [StringLength(500)]
    public string NoiDung { get; set; }

    [ForeignKey("Idnpl")]
    [InverseProperty("NoteNpls")]
    public virtual Npl IdnplNavigation { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

[Table("Image")]
public partial class Image
{
    [Key]
    [Column("IDImg")]
    public int Idimg { get; set; }

    [Column("URL")]
    [StringLength(500)]
    public string Url { get; set; }

    [Column("IDVariant")]
    public int Idvariant { get; set; }

    [Column("FileType")]
    public string FileType { get; set; }

    [StringLength(500)]
    public string NoiDung { get; set; }

    [ForeignKey("Idvariant")]
    [InverseProperty("Images")]
    public virtual ProductVariant IdvariantNavigation { get; set; }
}

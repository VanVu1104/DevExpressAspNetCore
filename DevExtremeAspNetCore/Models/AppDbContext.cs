using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DevExtremeAspNetCore.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Ctdh> Ctdhs { get; set; }

    public virtual DbSet<DonHangModels> DonHangs { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<ListNpl> ListNpls { get; set; }

    public virtual DbSet<NoteChiTietDonHang> NoteChiTietDonHangs { get; set; }


    public virtual DbSet<NoteNpl> NoteNpls { get; set; }

    public virtual DbSet<Npl> Npls { get; set; }

    public virtual DbSet<ProductModels> Products { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=QLMayMac;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Idcolor).HasName("PK__Color__E424D936A8E8FCC9");
        });

        modelBuilder.Entity<Ctdh>(entity =>
        {
            entity.HasKey(e => e.Idctdh).HasName("PK__CTDH__0F87803D8E91CC5D");

            entity.HasOne(d => d.IdcolorNavigation).WithMany(p => p.Ctdhs).HasConstraintName("FK_CTDH_Color");

            entity.HasOne(d => d.IddhNavigation).WithMany(p => p.Ctdhs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTDH__IDDH__33D4B598");

            entity.HasOne(d => d.IdproNavigation).WithMany(p => p.Ctdhs).HasConstraintName("FK_CTDH_Product");

            entity.HasOne(d => d.IdsizeNavigation).WithMany(p => p.Ctdhs).HasConstraintName("FK_CTDH_Size");
        });

        modelBuilder.Entity<DonHangModels>(entity =>
        {
            entity.HasKey(e => e.Iddh).HasName("PK__DonHang__B87DB898CBDF277F");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Idimg).HasName("PK__Image__9511D755D64F1225");

            entity.HasOne(d => d.IdvariantNavigation).WithMany(p => p.Images)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Image__IDVariant__2F10007B");
        });

        modelBuilder.Entity<ListNpl>(entity =>
        {
            entity.HasKey(e => e.Idlist).HasName("PK__ListNPL__F5D88C0DBA2934FC");

            entity.Property(e => e.Idvariant).HasDefaultValue(1);

            entity.HasOne(d => d.IdnplNavigation).WithMany(p => p.ListNpls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ListNPL__IDNPL__3D5E1FD2");

            entity.HasOne(d => d.IdvariantNavigation).WithMany(p => p.ListNpls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListNPL_ProductVariant");
        });

        modelBuilder.Entity<NoteChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.Idnote).HasName("PK__NoteChiT__E5F1D2E7E8D6853B");

            entity.HasOne(d => d.IdctdhNavigation).WithMany(p => p.NoteChiTietDonHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NoteChiTi__IDCTD__37A5467C");
        });

        modelBuilder.Entity<NoteNpl>(entity =>
        {
            entity.HasKey(e => e.IdnoteNpl).HasName("PK__NoteNPL__440A85EA9152B1FC");

            entity.HasOne(d => d.IdnplNavigation).WithMany(p => p.NoteNpls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NoteNPL__IDNPL__403A8C7D");
        });

        modelBuilder.Entity<Npl>(entity =>
        {
            entity.HasKey(e => e.Idnpl).HasName("PK__NPL__945ECD731ACBC4E7");
        });

        modelBuilder.Entity<ProductModels>(entity =>
        {
            entity.HasKey(e => e.Idpro).HasName("PK__Product__98F9285982DB1785");
        });
        // NoteChiTietDonHang entity configuration
        modelBuilder.Entity<NoteChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.Idnote).HasName("PK__NoteChiT__E5F1D2E7XXXXXXX");

            entity.Property(e => e.UrlFile).HasMaxLength(500);
            entity.Property(e => e.UrlImage).HasMaxLength(500);
            entity.Property(e => e.NoiDung).HasMaxLength(500);

            entity.HasOne(d => d.IdctdhNavigation)
                .WithMany(p => p.NoteChiTietDonHangs)
                .HasForeignKey(d => d.Idctdh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NoteChiTietDonHang__CTDH");
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.Idvariant).HasName("PK__ProductV__C019220C8E69C71C");

            entity.HasOne(d => d.IdcolorNavigation).WithMany(p => p.ProductVariants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__IDCol__2C3393D0");

            entity.HasOne(d => d.IdproNavigation).WithMany(p => p.ProductVariants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__IDPro__2A4B4B5E");

            entity.HasOne(d => d.IdsizeNavigation).WithMany(p => p.ProductVariants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__IDSiz__2B3F6F97");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Idsize).HasName("PK__Size__C4E3CC40482C162F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

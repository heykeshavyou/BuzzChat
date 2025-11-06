using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BuzzTalk.Data.Entities;

public partial class BuzzTalkContext : DbContext
{
    public BuzzTalkContext()
    {
    }

    public BuzzTalkContext(DbContextOptions<BuzzTalkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupUser> GroupUsers { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Groups__3214EC07BDD43F92");

            entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            entity.Property(e => e.Guid)
                .HasMaxLength(256)
                .HasColumnName("GUID");
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Groups__CreatedB__5CD6CB2B");
        });

        modelBuilder.Entity<GroupUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroupUse__3214EC071030B561");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupUsers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupUser__Group__619B8048");

            entity.HasOne(d => d.User).WithMany(p => p.GroupUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupUser__UserI__628FA481");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3214EC078969E066");

            entity.ToTable("Message");

            entity.Property(e => e.SentOn).HasColumnType("datetime");

            entity.HasOne(d => d.From).WithMany(p => p.MessageFroms)
                .HasForeignKey(d => d.FromId)
                .HasConstraintName("FK__Message__FromId__3A81B327");

            entity.HasOne(d => d.Group).WithMany(p => p.Messages)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__Message__GroupId__6383C8BA");

            entity.HasOne(d => d.To).WithMany(p => p.MessageTos)
                .HasForeignKey(d => d.ToId)
                .HasConstraintName("FK__Message__ToId__3B75D760");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07C0A2DEF1");

            entity.Property(e => e.JoinedOn).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

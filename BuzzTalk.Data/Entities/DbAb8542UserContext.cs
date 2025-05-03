using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BuzzTalk.Data.Entities;

public partial class DbAb8542UserContext : DbContext
{
    public DbAb8542UserContext()
    {
    }

    public DbAb8542UserContext(DbContextOptions<DbAb8542UserContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3214EC078969E066");

            entity.ToTable("Message");

            entity.Property(e => e.SentOn).HasColumnType("datetime");

            entity.HasOne(d => d.From).WithMany(p => p.MessageFroms)
                .HasForeignKey(d => d.FromId)
                .HasConstraintName("FK__Message__FromId__3A81B327");

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

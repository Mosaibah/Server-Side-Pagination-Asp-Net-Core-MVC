using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ServerSidePagination.Models
{
    public partial class ServerSidePaginationContext : DbContext
    {
        public ServerSidePaginationContext()
        {
        }

        public ServerSidePaginationContext(DbContextOptions<ServerSidePaginationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-22FSHU7\\SQLEXPRESS;Database=ServerSidePagination;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.CreatedOnUtc).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

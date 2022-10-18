using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LaptopShoppingSite.DAL.DomainClasses;

namespace LaptopShoppingSite.DAL
{
    public partial class StoreContext : DbContext
    {
        public StoreContext()
        {
        }

        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branch> Branches { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderLineItem> OrderLineItems { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectsV13;Database=CaseStudyDB;Trusted_Connection=True;");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(e => e.City)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Region)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Timer)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderAmount).HasColumnType("money");
            });

            modelBuilder.Entity<OrderLineItem>(entity =>
            {
                entity.HasIndex(e => e.OrderId, "IX_OrderLineItems_OrderId");

                entity.HasIndex(e => e.ProductId, "IX_OrderLineItems_ProductId");

                entity.Property(e => e.ProductId).HasMaxLength(15);

                entity.Property(e => e.SellingPrice).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLineItems)
                    .HasForeignKey(d => d.OrderId);

            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.BrandID, "IX_Products_BrandID");

                entity.Property(e => e.Id).HasMaxLength(15);

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.GraphicName).HasMaxLength(50);

                entity.Property(e => e.MRSP).HasColumnType("money");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.Timer)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.costPrice).HasColumnType("money");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Стройматериалы.Models;

namespace Стройматериалы.DB
{
    public partial class DEContext : DbContext
    {
        public DEContext()
        {
        }

        public DEContext(DbContextOptions<DEContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderProduct> OrderProducts { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<PickupPoint> PickupPoints { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<ProductManufacturer> ProductManufacturers { get; set; } = null!;
        public virtual DbSet<ProductSupplier> ProductSuppliers { get; set; } = null!;
        public virtual DbSet<ProductUnit> ProductUnits { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=192.168.1.15\\SQLEXPRESS;database=DE;user=student;password=student");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.OrderPickupPointId).HasColumnName("OrderPickupPointID");

                entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Order_User");

                entity.HasOne(d => d.OrderPickupPoint)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderPickupPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_PickupPoints");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_OrderStatus");
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductArticleNumber })
                    .HasName("PK__OrderPro__817A2662FC28DEAE");

                entity.ToTable("OrderProduct");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductArticleNumber).HasMaxLength(100);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderProd__Order__2D27B809");

                entity.HasOne(d => d.ProductArticleNumberNavigation)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.ProductArticleNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderProd__Produ__2E1BDC42");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("OrderStatus");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PickupPoint>(entity =>
            {
                entity.HasKey(e => e.PointId);

                entity.Property(e => e.PointId).HasColumnName("PointID");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductArticleNumber)
                    .HasName("PK__Product__2EA7DCD5E22AA871");

                entity.ToTable("Product");

                entity.Property(e => e.ProductArticleNumber).HasMaxLength(100);

                entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategoryID");

                entity.Property(e => e.ProductCost).HasColumnType("decimal(19, 4)");

                entity.Property(e => e.ProductManufacturerId).HasColumnName("ProductManufacturerID");

                entity.Property(e => e.ProductPhoto).HasColumnType("image");

                entity.Property(e => e.ProductUnitId).HasColumnName("ProductUnitID");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductCategory");

                entity.HasOne(d => d.ProductManufacturer)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductManufacturer");

                entity.HasOne(d => d.ProductUnit)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductUnit");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Product_ProductSupplier");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("ProductCategory");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            });

            modelBuilder.Entity<ProductManufacturer>(entity =>
            {
                entity.HasKey(e => e.ManufacturerId);

                entity.ToTable("ProductManufacturer");

                entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            });

            modelBuilder.Entity<ProductSupplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId);

                entity.ToTable("ProductSupplier");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.Title).IsUnicode(false);
            });

            modelBuilder.Entity<ProductUnit>(entity =>
            {
                entity.HasKey(e => e.UnitId);

                entity.ToTable("ProductUnit");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.Property(e => e.UserPatronymic).HasMaxLength(100);

                entity.Property(e => e.UserSurname).HasMaxLength(100);

                entity.HasOne(d => d.UserRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__UserRole__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

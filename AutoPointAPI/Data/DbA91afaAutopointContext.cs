using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AutoPointAPI.Data;

public partial class DbA91afaAutopointContext : DbContext
{
    public DbA91afaAutopointContext()
    {
    }

    public DbA91afaAutopointContext(DbContextOptions<DbA91afaAutopointContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartProduct> CartProducts { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_CI_AS");

        modelBuilder.Entity<CartProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CartProducts");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.ProductQuantity).HasColumnName("productQuantity");
            entity.Property(e => e.UserId).HasColumnName("userID");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Comments");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FullName).HasColumnName("fullName");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.ProductId).HasColumnName("productID");
        });

        modelBuilder.Entity<MigrationHistory>(entity =>
        {
            entity.HasKey(e => new { e.MigrationId, e.ContextKey }).HasName("PK_dbo.__MigrationHistory");

            entity.ToTable("__MigrationHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ContextKey).HasMaxLength(300);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Orders");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddressOne).HasColumnName("addressOne");
            entity.Property(e => e.AddressTwo).HasColumnName("addressTwo");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.CompanyName).HasColumnName("companyName");
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("deliveryDate");
            entity.Property(e => e.DeliveryType).HasColumnName("deliveryType");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FullName).HasColumnName("fullName");
            entity.Property(e => e.PaymentMethod).HasColumnName("paymentMethod");
            entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.ProductIds).HasColumnName("productIDs");
            entity.Property(e => e.ProductQuantities).HasColumnName("productQuantities");
            entity.Property(e => e.ProductsCount).HasColumnName("productsCount");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.UserId).HasColumnName("userID");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Products");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.TypeOfProduct).HasColumnName("typeOfProduct");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Users");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.Password).HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

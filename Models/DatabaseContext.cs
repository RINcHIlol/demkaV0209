using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace demo020925.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Benefit> Benefits { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubOrder> SubOrders { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SubscriptionsCategory> SubscriptionsCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersBenefit> UsersBenefits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=5432");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Benefit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("benefits_pkey");

            entity.ToTable("benefits");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.OrderTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_time");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("orders_customer_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SubOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sub_order_pkey");

            entity.ToTable("sub_order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.SubId).HasColumnName("sub_id");
            entity.Property(e => e.TrainerId).HasColumnName("trainer_id");

            entity.HasOne(d => d.Order).WithMany(p => p.SubOrders)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("sub_order_order_id_fkey");

            entity.HasOne(d => d.Sub).WithMany(p => p.SubOrders)
                .HasForeignKey(d => d.SubId)
                .HasConstraintName("sub_order_sub_id_fkey");

            entity.HasOne(d => d.Trainer).WithMany(p => p.SubOrders)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("sub_order_trainer_id_fkey");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subscribtions_pkey");

            entity.ToTable("subscriptions");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('subscribtions_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.DurationPerMonths).HasColumnName("duration_per_months");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.Category).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("subscriptions_category_id_fkey");
        });

        modelBuilder.Entity<SubscriptionsCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subscriptions_categories_pkey");

            entity.ToTable("subscriptions_categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CurrentSubsriptionId).HasColumnName("current_subsription_id");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.VisitDurationPerMonts).HasColumnName("visit_duration_per_monts");

            entity.HasOne(d => d.CurrentSubsription).WithMany(p => p.Users)
                .HasForeignKey(d => d.CurrentSubsriptionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("users_current_subsription_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<UsersBenefit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_benefits_pkey");

            entity.ToTable("users_benefits");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BenefitId).HasColumnName("benefit_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Benefit).WithMany(p => p.UsersBenefits)
                .HasForeignKey(d => d.BenefitId)
                .HasConstraintName("users_benefits_benefit_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UsersBenefits)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("users_benefits_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

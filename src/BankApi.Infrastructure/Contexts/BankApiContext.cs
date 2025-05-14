using System;
using System.Collections.Generic;
using BankApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Contexts;

public partial class BankApiContext : DbContext
{
    public BankApiContext()
    {
    }

    public BankApiContext(DbContextOptions<BankApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<BankTransaction> BankTransactions { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.ToTable("BankAccount");

            entity.HasIndex(e => e.Id, "pk_BankAccount").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("BIGINT");
            entity.Property(e => e.AccountNumber).HasColumnType("INT");
            entity.Property(e => e.Balance).HasColumnType("DECIMAL(4)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
            entity.Property(e => e.CustomerId).HasColumnType("BIGINT");
            entity.Property(e => e.InteresRate).HasColumnType("DECIMAL(2)");
            entity.Property(e => e.InterestType).HasColumnType("CHAR(1)");
            entity.Property(e => e.UpdatedAt).HasColumnType("DATETIME");

            entity.HasOne(d => d.Customer).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<BankTransaction>(entity =>
        {
            entity.ToTable("BankTransaction");

            entity.HasIndex(e => e.Id, "pk_BankTransaction").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("BIGINT");
            entity.Property(e => e.BankAccountId).HasColumnType("BIGINT");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
            entity.Property(e => e.TransactionType).HasColumnType("CHAR(1)");
            entity.Property(e => e.UpdatedAt).HasColumnType("DATETIME");

            entity.HasOne(d => d.BankAccount).WithMany(p => p.BankTransactions)
                .HasForeignKey(d => d.BankAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("BIGINT");
            entity.Property(e => e.BirthDate).HasColumnType("DATETIME");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
            entity.Property(e => e.Gender).HasColumnType("CHAR(1)");
            entity.Property(e => e.Income).HasColumnType("DECIMAL(2)");
            entity.Property(e => e.LastName).HasColumnType("ENUM");
            entity.Property(e => e.UpdatedAt).HasColumnType("DATE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

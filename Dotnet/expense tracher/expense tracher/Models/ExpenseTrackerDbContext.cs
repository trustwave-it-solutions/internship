using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace expense_tracher.Models;

public partial class ExpenseTrackerDbContext : DbContext
{
    public ExpenseTrackerDbContext()
    {
    }

    public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblPaymentMode> TblPaymentModes { get; set; }

    public virtual DbSet<TblPaymentType> TblPaymentTypes { get; set; }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=expensetrakerdb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblPaymentType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TblTransaction>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.TblTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblTransaction_TblCategory");

            entity.HasOne(d => d.PaymentMode).WithMany(p => p.TblTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblTransaction_TblPaymentMode");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.TblTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblTransaction_TblPaymentType");

            entity.HasOne(d => d.User).WithMany(p => p.TblTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblTransaction_TblUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

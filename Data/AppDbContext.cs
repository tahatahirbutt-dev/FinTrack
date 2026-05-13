using FinTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<BudgetLimit> Budgets => Set<BudgetLimit>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        builder.Entity<Expense>()
            .HasOne(e => e.User)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.UserId);

        builder.Entity<BudgetLimit>()
            .HasOne(b => b.User)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.UserId);

        builder.Entity<Expense>()
            .Property(e => e.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<BudgetLimit>()
            .Property(b => b.MonthlyLimit)
            .HasColumnType("decimal(18,2)");
    }
}

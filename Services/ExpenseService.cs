using FinTrack.Data;
using FinTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Services;

public class ExpenseService
{
    private readonly AppDbContext _db;

    public ExpenseService(AppDbContext db) => _db = db;

    public async Task<List<Expense>> GetExpensesAsync(int userId, int? month = null, int? year = null)
    {
        var query = _db.Expenses.AsNoTracking().Where(e => e.UserId == userId);

        if (month.HasValue && year.HasValue)
            query = query.Where(e => e.Date.Month == month && e.Date.Year == year);

        return await query.OrderByDescending(e => e.Date).ToListAsync();
    }

    public async Task AddExpenseAsync(Expense expense)
    {
        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateExpenseAsync(Expense expense)
    {
        _db.Expenses.Update(expense);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteExpenseAsync(int id)
    {
        var exp = await _db.Expenses.FindAsync(id);
        if (exp != null)
        {
            _db.Expenses.Remove(exp);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<BudgetLimit>> GetBudgetsAsync(int userId, int month, int year)
        => await _db.Budgets.AsNoTracking()
            .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
            .ToListAsync();

    public async Task SetBudgetAsync(BudgetLimit budget)
    {
        var existing = await _db.Budgets.FirstOrDefaultAsync(b =>
            b.UserId == budget.UserId &&
            b.Category == budget.Category &&
            b.Month == budget.Month &&
            b.Year == budget.Year);

        if (existing == null)
            _db.Budgets.Add(budget);
        else
        {
            existing.MonthlyLimit = budget.MonthlyLimit;
            _db.Budgets.Update(existing);
        }

        await _db.SaveChangesAsync();
    }

    public static readonly string[] Categories = new[]
    {
        "Food", "Transport", "Shopping", "Utilities",
        "Health", "Entertainment", "Education", "Other"
    };
}

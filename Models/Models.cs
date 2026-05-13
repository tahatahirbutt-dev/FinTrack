namespace FinTrack.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<Expense> Expenses { get; set; } = new();
    public List<BudgetLimit> Budgets { get; set; } = new();
}

public class Expense
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Category { get; set; } = "Other";
    public DateTime Date { get; set; } = DateTime.Today;
    public string Notes { get; set; } = string.Empty;
    public User? User { get; set; }
}

public class BudgetLimit
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal MonthlyLimit { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public User? User { get; set; }
}

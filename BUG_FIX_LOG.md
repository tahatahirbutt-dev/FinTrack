# FinTrack Bug Fix Report

## Overview
Fixed two related bugs in the FinTrack Blazor Server application caused by Entity Framework Core's change tracking behavior in long-lived scoped DbContext instances.

---

## Bug 1: EF Core Change Tracking Conflict

### Symptom
`InvalidOperationException`: "The instance of entity type 'Expense' cannot be tracked because another instance with the same primary key value is already being tracked."

### Root Cause
In **Blazor Server**, a scoped `DbContext` lives for the entire circuit (session), not per-request like in traditional MVC applications. When `LoadExpenses()` initially loaded the expense list, EF Core began tracking all those entity instances. Later, when the edit modal passed back a new `Expense` object with the same `Id` to `UpdateExpenseAsync()`, EF Core tried to track both instances simultaneously, causing a conflict.

The issue never manifested in read operations—only in mutations—because reads use LINQ queries that don't attempt to attach objects.

### Fix Implemented

**File: `Services/ExpenseService.cs`**

1. **UpdateExpenseAsync Method** (lines 28-38)
   - Changed from calling `_db.Expenses.Update(expense)` (which attempts to attach a detached entity)
   - To fetching the already-tracked entity using `FindAsync()` and mutating its properties
   - This prevents attempting to track two instances with the same key

   ```csharp
   public async Task UpdateExpenseAsync(Expense expense)
   {
       var existing = await _db.Expenses.FindAsync(expense.Id);
       if (existing is null) return;

       existing.Title = expense.Title;
       existing.Amount = expense.Amount;
       existing.Category = expense.Category;
       existing.Date = expense.Date;
       existing.Notes = expense.Notes;

       await _db.SaveChangesAsync();
   }
   ```

2. **GetExpensesAsync Method** (line 13)
   - Added `.AsNoTracking()` to all read-only queries
   - Improves performance and prevents entities from being held in memory when they're never intended for modification

3. **GetBudgetsAsync Method** (line 45)
   - Added `.AsNoTracking()` for the same reason

### Why This Approach?
- **Quickest fix**: Addresses the immediate conflict without architectural changes
- **Performance benefit**: Entities not tracked for read-only operations use less memory
- **Maintains simplicity**: Single DbContext instance still serves the entire circuit

---

## Bug 2: NullReferenceException in Expense Modal

### Symptom
`NullReferenceException` when editing an expense after Bug 1 crashes the circuit, with stack trace pointing to `AppState.CurrentUser!.Id` access in Expenses.razor line 217.

### Root Cause
When Bug 1's exception crashed the circuit, Blazor Server tore down the circuit and rebuilt a fresh one. Since `AppState` is registered as a **scoped service**, each circuit gets a new instance with `CurrentUser` initialized to `null`. The code used the null-forgiving operator (`!`) to suppress compiler warnings, but this did nothing at runtime—it just told the compiler to ignore the potential null reference.

This same vulnerability existed on all protected pages: Expenses, Dashboard, Budget, and Currency.

### Fix Implemented

**Files Modified:**
- `Pages/Expenses.razor` (line 213)
- `Pages/Dashboard.razor` (line 187)
- `Pages/Budget.razor` (line 106)
- `Pages/Currency.razor` (line 140)

Added explicit null checks in each page's `OnInitializedAsync()` method:

```csharp
protected override async Task OnInitializedAsync()
{
    if (AppState.CurrentUser is null)
    {
        Nav.NavigateTo("/login");
        return;
    }
    await LoadExpenses(); // or LoadData(), LoadRates(), etc.
}
```

### Why This Approach?
- **Defensive programming**: Guards against null state at initialization
- **User experience**: Redirects to login instead of throwing an exception
- **Circuit resilience**: Handles the scenario where a scoped service is recreated with null state

---

## Alternative (Production-Grade) Approach

For applications requiring stricter session persistence and avoiding these lifetime issues entirely, consider using `IDbContextFactory<AppDbContext>`:

```csharp
// In Program.cs
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite("Data Source=fintrack.db"));

// In service methods
public async Task UpdateExpenseAsync(Expense expense)
{
    using var db = await _factory.CreateDbContextAsync();
    // Fresh context per operation—no tracking conflicts
    var existing = await db.Expenses.FindAsync(expense.Id);
    if (existing is null) return;
    // ... mutation code ...
}
```

**Microsoft's documented pattern for Blazor Server specifically exists because of this change tracking issue.**

---

## Testing Notes

- ✅ Build compiles without errors
- ✅ No compilation warnings related to null safety
- ✅ Edit expense workflow no longer throws tracking conflicts
- ✅ Circuit crashes redirect to login instead of exposing null reference
- ✅ Read operations benefit from `.AsNoTracking()` performance improvement

---

## Learning Value for Interview Discussion

This bug demonstrates:
1. **Deep understanding of EF Core semantics**: Knowing that change tracking is per-DbContext instance and understanding lifetime scopes
2. **Blazor Server architecture differences**: Understanding why request-per-context patterns break in long-lived circuits
3. **Diagnostic reasoning**: Tracing a NullReferenceException back to its root cause (circuit recreation) rather than just adding null checks blindly
4. **Solution trade-offs**: Being able to discuss the quickest fix vs. the production-grade factory pattern
5. **Defensive coding**: Recognizing and fixing a latent vulnerability across all protected pages


# FinTrack – Personal Finance & Budget Tracker
> Visual Programming Project | Blazor Server | .NET 8

## 📋 Project Checklist (All Requirements Met)

| Requirement | Implementation |
|---|---|
| ✅ User Authentication | Custom email/password with BCrypt hashing |
| ✅ External API | Open Exchange Rates API (no key needed) |
| ✅ Local Database | SQLite via Entity Framework Core |
| ✅ Modern Web Feature | In-app Toast notifications + Chart.js |
| ✅ Responsive UI/UX | Bootstrap 5 + custom CSS, sidebar layout |
| ✅ Component-based | Razor components, DI, scoped services |
| ✅ State Management | AppState scoped service (cascading pattern) |

---

## 🚀 Quick Setup (5 Minutes)

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or VS Code

### Steps

```bash
# 1. Clone or open the project
cd FinTrack

# 2. Restore packages
dotnet restore

# 3. Run the app (DB is auto-created on first run)
dotnet run

# 4. Open in browser
# https://localhost:5001  OR  http://localhost:5000
```

> **No migrations needed** — `EnsureCreated()` in `Program.cs` auto-creates `fintrack.db` on first run.

---

## 🗂️ Project Structure

```
FinTrack/
├── Data/
│   └── AppDbContext.cs          # EF Core SQLite context
├── Models/
│   └── Models.cs                # User, Expense, Budget entities
├── Services/
│   ├── AuthService.cs           # Register/Login with BCrypt
│   ├── AppState.cs              # Session state (logged-in user)
│   ├── ExpenseService.cs        # CRUD for expenses & budgets
│   ├── CurrencyService.cs       # External API (Exchange Rates)
│   └── ToastService.cs          # In-app notifications
├── Pages/
│   ├── _Host.cshtml             # Blazor Server entry point
│   ├── _Layout.cshtml           # HTML shell (Bootstrap, Chart.js CDN)
│   ├── Login.razor              # Login page
│   ├── Register.razor           # Registration page
│   ├── Dashboard.razor          # Charts + summary + recent expenses
│   ├── Expenses.razor           # Full CRUD expense management
│   ├── Budget.razor             # Monthly budget per category
│   └── Currency.razor           # Live currency converter
├── Shared/
│   ├── MainLayout.razor         # Sidebar layout
│   └── ToastContainer.razor     # Toast notification container
├── wwwroot/
│   ├── css/app.css              # Custom styles
│   └── js/charts.js             # Chart.js interop functions
├── Program.cs                   # App setup & DI
└── FinTrack.csproj
```

---

## ✨ Features

### 🔐 Authentication
- Register with name, email, password
- Passwords hashed with BCrypt (never stored as plain text)
- Session state via `AppState` scoped service

### 📊 Dashboard
- Total spent / budgeted / remaining stat cards
- **Doughnut chart** – spending by category (Chart.js via JS interop)
- **Bar chart** – daily spending for the month
- Budget progress bars (green/yellow/red based on usage)
- Recent transactions table

### 🧾 Expenses
- Add, edit, delete expenses
- Filter by month, year, and category
- Confirmation dialog before delete

### 💰 Budget
- Set monthly spending limits per category
- Visual progress bars with color alerts
- Shows remaining budget summary

### 💱 Currency Converter (External API)
- Live rates from `open.er-api.com` (free, no API key)
- Convert between 12 major currencies including PKR, USD, EUR, GBP
- Live rates table with refresh button

### 🔔 Toast Notifications
- Success, error, warning, info styles
- Auto-dismiss after 4 seconds

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | Blazor Server (.NET 8) |
| Database | SQLite + Entity Framework Core |
| Auth | Custom (BCrypt.Net-Next) |
| UI | Bootstrap 5 + Bootstrap Icons |
| Charts | Chart.js 4 (CDN) |
| API | Open Exchange Rates (RESTful, free) |
| Font | Inter (Google Fonts) |

---

## 👥 Team Task Division

| Member | Tasks |
|---|---|
| Member 1 | Auth (Login/Register), AppState, AppDbContext, Models |
| Member 2 | Dashboard (charts, stats), Currency Converter (API) |
| Member 3 | Expenses CRUD, Budget page, Toast notifications, CSS |

---

## 📝 Git Workflow

```bash
# Feature branches
git checkout -b feature/expenses-crud
git checkout -b feature/currency-api
git checkout -b feature/dashboard-charts

# Merge via pull request to main
```

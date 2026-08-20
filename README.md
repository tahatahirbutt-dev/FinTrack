# FinTrack — Personal Finance & Budget Tracker

**A Blazor Server application for tracking expenses, setting monthly category budgets, and converting currency against live exchange rates. Built on .NET 8 with Entity Framework Core and SQLite.**

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![Blazor](https://img.shields.io/badge/Blazor-Server-5C2D91)
![EF Core](https://img.shields.io/badge/EF%20Core-SQLite-blue)
![License](https://img.shields.io/badge/License-MIT-green)

---

## Overview

FinTrack answers three questions a person actually has about their money: where
did it go, am I over budget, and what is this worth in another currency.

It's a full-stack Blazor Server application — authentication, persistent
storage, an analytics dashboard, and a live external API integration — built as
a university Visual Programming project and developed end to end by me.

### Screenshots

**Dashboard** — spend summary, category breakdown and daily spending

![Dashboard](docs/screenshots/dashboard.png)

**Budget vs. spending and recent transactions**

![Budget progress](docs/screenshots/dashboard-budgets.png)

| Expenses | Monthly budgets | Currency converter |
|---|---|---|
| ![Expenses](docs/screenshots/expenses.png) | ![Budget](docs/screenshots/budget.png) | ![Currency](docs/screenshots/currency.png) |

---

## Features

### Authentication
Custom email/password registration and login. Passwords are hashed with
**BCrypt** and never stored in plain text. Session state is held in a scoped
`AppState` service and shared down the component tree using Blazor's
cascading-value pattern.

### Dashboard
- Summary cards: total spent, total budgeted, remaining, transaction count
- **Doughnut chart** — spending by category
- **Bar chart** — daily spending across the current month
- Budget progress bars that shift green → yellow → red as a category's limit
  is approached
- Recent transactions table

Both charts are rendered with Chart.js through Blazor's **JavaScript interop**,
since Chart.js is a JS library with no native Blazor equivalent in this project.

### Expenses
Full CRUD — add, edit and delete expenses — with filtering by month, year and
category, and a confirmation dialog before any destructive action.

### Budgets
Monthly spending limits set per category, with visual progress indicators and a
remaining-budget summary.

### Currency Converter
Live rates pulled from the **Open Exchange Rates API** (`open.er-api.com`),
converting between 12 major currencies including PKR, USD, EUR and GBP. The
endpoint requires no API key, so the feature works on a fresh clone with no
configuration.

### Toast Notifications
In-app success/error/warning/info toasts with auto-dismiss after 4 seconds,
driven by a scoped `ToastService` and a shared `ToastContainer` component.

---

## Architecture

```
FinTrack/
├── Data/
│   └── AppDbContext.cs        # EF Core DbContext (SQLite)
├── Models/
│   └── Models.cs              # User, Expense, Budget entities
├── Services/
│   ├── AuthService.cs         # Registration & login, BCrypt hashing
│   ├── AppState.cs            # Scoped session state
│   ├── ExpenseService.cs      # Expense & budget CRUD
│   ├── CurrencyService.cs     # External exchange-rate API client
│   └── ToastService.cs        # Notification queue
├── Pages/
│   ├── _Host.cshtml           # Blazor Server entry point
│   ├── _Layout.cshtml         # HTML shell (Bootstrap, Chart.js)
│   ├── Login.razor
│   ├── Register.razor
│   ├── Dashboard.razor
│   ├── Expenses.razor
│   ├── Budget.razor
│   └── Currency.razor
├── Shared/
│   ├── MainLayout.razor       # Sidebar layout
│   └── ToastContainer.razor
├── wwwroot/
│   ├── css/app.css
│   └── js/charts.js           # Chart.js interop functions
└── Program.cs                 # DI registration & app setup
```

**Design decisions worth naming:**

- **Services, not code-behind.** All business logic lives in injected scoped
  services rather than in the Razor components, so pages stay presentational and
  logic is testable independently of the UI.
- **Scoped state over static.** `AppState` is registered scoped, which in Blazor
  Server means per-user-circuit — a static field would have leaked one user's
  session into every other user's.
- **`EnsureCreated()` over migrations.** The database is provisioned on first
  run, so a clone works immediately with no migration step. See *Known
  limitations* for the trade-off this makes.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | Blazor Server (.NET 8) |
| ORM | Entity Framework Core |
| Database | SQLite |
| Authentication | Custom, BCrypt.Net-Next |
| UI | Bootstrap 5, Bootstrap Icons |
| Charts | Chart.js 4 via JS interop |
| External API | Open Exchange Rates (`open.er-api.com`) |
| Font | Inter (Google Fonts) |

---

## Running It

**Prerequisites:** .NET 8 SDK. Visual Studio 2022 or VS Code optional.

```bash
git clone https://github.com/tahatahirbutt-dev/FinTrack.git
cd FinTrack
dotnet restore
dotnet run
```

Then open `https://localhost:5001` (or `http://localhost:5000`).

No database setup and no migrations — `EnsureCreated()` in `Program.cs` builds
`fintrack.db` on first run. Register a new account through the UI to get started.

---

## Known Limitations

Documented deliberately rather than left for a reader to discover:

- **`EnsureCreated()` instead of EF migrations.** This makes a fresh clone work
  with zero setup, but it means the schema can't be versioned or evolved
  incrementally — a schema change requires deleting the database. A production
  version would use `dotnet ef migrations`.
- **No automated tests.** The application was verified manually across
  registration, expense CRUD, budget alerts and currency conversion. No unit or
  integration test suite exists yet.
- **Session state is not persistent.** `AppState` lives in the Blazor Server
  circuit, so a page refresh or connection drop logs the user out. Persisting
  authentication would require cookie or token-based auth.
- **External API has no caching or rate-limit handling.** Every conversion hits
  `open.er-api.com` directly. A production version would cache rates and degrade
  gracefully when the API is unreachable.
- **No multi-currency storage.** Expenses are stored in a single implicit
  currency; the converter is a standalone utility rather than being wired into
  expense records.

---

## License

MIT License © 2026 Taha Tahir Butt

---

*Built and developed end to end by Taha Tahir Butt — Rawalpindi, Pakistan.*

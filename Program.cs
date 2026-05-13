using FinTrack.Data;
using FinTrack.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor Server services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// SQLite database via Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=fintrack.db"));

// Custom services — Scoped so each user session gets its own instance
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<ToastService>();

// HttpClient for Currency API
builder.Services.AddHttpClient<CurrencyService>();

var app = builder.Build();

// Auto-create the SQLite database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

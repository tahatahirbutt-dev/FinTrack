using FinTrack.Models;

namespace FinTrack.Services;

/// <summary>
/// Scoped service to hold the currently logged-in user for the session.
/// Injected as cascading value so all components can access it.
/// </summary>
public class AppState
{
    public User? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;

    public event Action? OnChange;

    public void Login(User user)
    {
        CurrentUser = user;
        NotifyStateChanged();
    }

    public void Logout()
    {
        CurrentUser = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}

namespace FinTrack.Services;

public enum ToastType { Success, Error, Warning, Info }

public class ToastMessage
{
    public string Message { get; set; } = string.Empty;
    public ToastType Type { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>
/// In-app notification service (toast/alert notifications — required feature)
/// </summary>
public class ToastService
{
    public event Action<ToastMessage>? OnShow;

    public void ShowSuccess(string message) => Show(message, ToastType.Success);
    public void ShowError(string message) => Show(message, ToastType.Error);
    public void ShowWarning(string message) => Show(message, ToastType.Warning);
    public void ShowInfo(string message) => Show(message, ToastType.Info);

    private void Show(string message, ToastType type)
        => OnShow?.Invoke(new ToastMessage { Message = message, Type = type });
}

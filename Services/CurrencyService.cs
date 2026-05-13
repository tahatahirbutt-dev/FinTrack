using System.Text.Json;

namespace FinTrack.Services;

public class CurrencyService
{
    private readonly HttpClient _http;
    // Free API — no key needed
    private const string BaseUrl = "https://open.er-api.com/v6/latest/";

    public CurrencyService(HttpClient http) => _http = http;

    public async Task<Dictionary<string, decimal>?> GetRatesAsync(string baseCurrency = "USD")
    {
        try
        {
            var response = await _http.GetStringAsync($"{BaseUrl}{baseCurrency}");
            using var doc = JsonDocument.Parse(response);
            var rates = doc.RootElement.GetProperty("rates");

            var result = new Dictionary<string, decimal>();
            foreach (var rate in rates.EnumerateObject())
                result[rate.Name] = rate.Value.GetDecimal();

            return result;
        }
        catch
        {
            return null;
        }
    }

    public static readonly string[] SupportedCurrencies = new[]
    {
        "USD", "EUR", "GBP", "PKR", "SAR", "AED",
        "INR", "CAD", "AUD", "JPY", "CNY", "TRY"
    };
}

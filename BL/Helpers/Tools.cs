using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Helpers;

public static class Tools
{
    private static IDal s_dal = Factory.Get; //stage 4

    public static BO.Status CalculateStatus(DO.Call call)
    {
        TimeSpan riskRange = s_dal.Config.RiskRange;

        if (call.MaxTimeFinishRead.HasValue)
        {
            TimeSpan remaining = call.MaxTimeFinishRead.Value - AdminManager.Now;

            if (remaining < TimeSpan.Zero)
                return BO.Status.Expired;

            if (remaining <= riskRange)
                return BO.Status.OpenAtRisk;

            return BO.Status.InProgress;
        }

        return BO.Status.InProgress;
    }


    private static string apiKey = "PK.83B935C225DF7E2F9B1ee90A6B46AD86";
    // private static readonly string apiKey1 = "vaUo0LbTQF27M9LVCg8w2b35GKIAJJyl";
    //public static (double, double) GetCoordinates(string address)
    //{
    //    using var client = new HttpClient();
    //    string url = $"https://us1.locationiq.com/v1/search.php?key={apiKey}&q={Uri.EscapeDataString(address)}&format=json";

    //    var response = client.GetAsync(url).GetAwaiter().GetResult();
    //    //if (!response.IsSuccessStatusCode)
    //    //    throw new Exception("Invalid address or API error.");

    //    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    //    using var doc = JsonDocument.Parse(json);

    //    if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
    //        throw new Exception("Address not found.");

    //    var root = doc.RootElement[0];

    //    if (!root.TryGetProperty("lat", out var latProperty) ||
    //        !root.TryGetProperty("lon", out var lonProperty))
    //    {
    //        throw new Exception("Missing latitude or longitude in response.");
    //    }

    //    if (!double.TryParse(latProperty.GetString(), out double latitude) ||
    //        !double.TryParse(lonProperty.GetString(), out double longitude))
    //    {
    //        throw new Exception("Invalid latitude or longitude format.");
    //    }

    //    return (latitude, longitude);
    //}
    public static (double, double) GetCoordinates(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new BO.BlInvalidInputException("Address cannot be empty.");

        using var client = new HttpClient();
        string url = $"https://us1.locationiq.com/v1/search.php?key={apiKey}&q={Uri.EscapeDataString(address)}&format=json";

        var response = client.GetAsync(url).GetAwaiter().GetResult();

        if (!response.IsSuccessStatusCode)
            throw new BO.BlInvalidInputException("Invalid address or API error.");

        var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
            throw new BO.BlInvalidInputException("Address not found.");

        var root = doc.RootElement[0];

        if (!root.TryGetProperty("lat", out var latProperty) ||
            !root.TryGetProperty("lon", out var lonProperty))
        {
            throw new BO.BlInvalidInputException("Missing latitude or longitude in response.");
        }

        if (!double.TryParse(latProperty.GetString(), out double latitude) ||
            !double.TryParse(lonProperty.GetString(), out double longitude))
        {
            throw new BO.BlInvalidInputException("Invalid latitude or longitude format.");
        }

        return (latitude, longitude);
    }



    /// <summary>
    /// Converts the properties of an object to a string representation.
    /// </summary>
    /// <returns>A string representation of the object's properties.</returns>
    public static string ToStringProperty<T>(this T t)
    {
        if (t == null) return "null";

        var properties = typeof(T).GetProperties();
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var property in properties)
        {
            var value = property.GetValue(t);
            stringBuilder.AppendLine($"{property.Name}: {value}");
        }

        return stringBuilder.ToString();
    }
    //public static (double, double) GetCoordinates(string address)
    //{
    //    string apiKey = "PK.83B935C225DF7E2F9B1ee90A6B46AD86";
    //    using var client = new HttpClient();
    //    string url = $"https://us1.locationiq.com/v1/search.php?key={apiKey}&q={Uri.EscapeDataString(address)}&format=json";

    //    var response = client.GetAsync(url).GetAwaiter().GetResult();
    //    if (!response.IsSuccessStatusCode)
    //        throw new BO.BlInvalidInputException("Invalid address or API error.");

    //    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    //    using var doc = JsonDocument.Parse(json);

    //    if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
    //        throw new BO.BlInvalidInputException("Address not found."); //???????????

    //    var root = doc.RootElement[0];

    //    return (root.GetProperty("lat").GetDouble(), root.GetProperty("lon").GetDouble());
    //}
    internal static double DistanceCalculation(string address1, string address2)
    {
        var (latitude1, longitude1) = GetCoordinates(address1);
        var (latitude2, longitude2) = GetCoordinates(address2);

        double dLat = latitude2 - latitude1;
        double dLon = longitude2 - longitude1;
        return Math.Sqrt(dLat * dLat + dLon * dLon) * 111;
    }

    private const string LOCATIONIQ_API_KEY = "your_api_key_here"; // Replace with your LocationIQ API key
    private const string GEOCODING_API_URL = "https://us1.locationiq.com/v1/search.php";

    //internal static (double latitude, double longitude) GetCoordinates(string address)
    //{
    //    if (string.IsNullOrWhiteSpace(address))
    //    {
    //        throw new ArgumentException("Address cannot be null or empty", nameof(address));
    //    }

    //    try
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            // Build the API request URL
    //            var requestUrl = $"{GEOCODING_API_URL}?key={LOCATIONIQ_API_KEY}&q={Uri.EscapeDataString(address)}&format=json";

    //            // Make synchronous request (no async/await)
    //            var response = client.GetStringAsync(requestUrl).Result;  // Here we use .Result to make it synchronous

    //            // Parse JSON response
    //            using (JsonDocument document = JsonDocument.Parse(response))
    //            {
    //                // LocationIQ returns an array of results, we take the first one
    //                var firstResult = document.RootElement.EnumerateArray().First();

    //                var lat = firstResult.GetProperty("lat").GetString();
    //                var lon = firstResult.GetProperty("lon").GetString();

    //                if (double.TryParse(lat, out double latitude) &&
    //                    double.TryParse(lon, out double longitude))
    //                {
    //                    return (latitude, longitude);
    //                }

    //                throw new InvalidOperationException("Failed to parse coordinates from API response");
    //            }
    //        }
    //    }
    //    catch (HttpRequestException ex)
    //    {
    //        throw new InvalidOperationException($"Failed to geocode address: {ex.Message}", ex);
    //    }
    //    catch (JsonException ex)
    //    {
    //        throw new InvalidOperationException($"Failed to parse geocoding response: {ex.Message}", ex);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new InvalidOperationException($"Unexpected error during geocoding: {ex.Message}", ex);
    //    }
    //}










    /// /מאפרת וחני
    //public static (double, double) GetCoordinatesFromAddress(string address)
    //{
    //    string apiKey = "PK.83B935C225DF7E2F9B1ee90A6B46AD86";
    //    using var client = new HttpClient();
    //    string url = $"https://us1.locationiq.com/v1/search.php?key={apiKey}&q={Uri.EscapeDataString(address)}&format=json";

    //    var response = client.GetAsync(url).GetAwaiter().GetResult();
    //    if (!response.IsSuccessStatusCode)
    //        throw new BO.BlInvalidInputException("Invalid address or API error.");

    //    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    //    using var doc = JsonDocument.Parse(json);

    //    if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
    //        throw new BO.BlInvalidInputException("Address not found.");

    //    var root = doc.RootElement[0];

    //    if (!root.TryGetProperty("lat", out var latElement) || !root.TryGetProperty("lon", out var lonElement))
    //        throw new BO.BlInvalidInputException("Latitude or longitude not found in response.");

    //    if (!double.TryParse(latElement.GetString(), out double latitude) ||
    //        !double.TryParse(lonElement.GetString(), out double longitude))
    //        throw new BO.BlInvalidInputException("Invalid latitude or longitude format.");
    //    var latitude = 0;
    //    var longitude = 0;
    //    return (latitude, longitude);
    //}
}
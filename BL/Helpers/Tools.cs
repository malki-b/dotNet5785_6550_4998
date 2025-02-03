using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Helpers;

 internal static class Tools
{
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
    public static (double, double) GetCoordinatesFromAddress(string address)
    {
        string apiKey = "PK.83B935C225DF7E2F9B1ee90A6B46AD86";
        using var client = new HttpClient();
        string url = $"https://us1.locationiq.com/v1/search.php?key={apiKey}&q={Uri.EscapeDataString(address)}&format=json";

        var response = client.GetAsync(url).GetAwaiter().GetResult();
        if (!response.IsSuccessStatusCode)
            throw new BO.BlInvalidInputException("Invalid address or API error.");

        var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
            throw new BO.BlInvalidInputException("Address not found."); //???????????

        var root = doc.RootElement[0];

        return (root.GetProperty("lat").GetDouble(), root.GetProperty("lon").GetDouble());
    }
    internal static double DistanceCalculation(string address1, string address2)
    {
        var (latitude1, longitude1) = GetCoordinatesFromAddress(address1);
        var (latitude2, longitude2) = GetCoordinatesFromAddress(address2);

        double dLat = latitude2 - latitude1;
        double dLon = longitude2 - longitude1;
        return Math.Sqrt(dLat * dLat + dLon * dLon) * 111;
    }
}

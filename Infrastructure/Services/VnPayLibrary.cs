using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

/// <summary>
/// Builds and verifies VNPay request/response signatures, mirroring VNPay's official
/// C# sample: parameters are sorted by key, URL-encoded, joined into a query string,
/// and signed with HMAC-SHA512 over that string using vnp_HashSecret.
/// WebUtility.UrlEncode is used (UPPERCASE %XX, space as '+') so the encoding matches
/// VNPay's Java backend exactly — lowercase %xx (HttpUtility) yields an invalid signature.
/// </summary>
public class VnPayLibrary
{
    private readonly SortedList<string, string> _requestData = new(StringComparer.Ordinal);
    private readonly SortedList<string, string> _responseData = new(StringComparer.Ordinal);

    public void AddRequestData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
            _requestData[key] = value;
    }

    public void AddResponseData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
            _responseData[key] = value;
    }

    public string GetResponseData(string key)
        => _responseData.TryGetValue(key, out var v) ? v : string.Empty;

    /// <summary>Builds the full payment URL with the appended vnp_SecureHash.</summary>
    public string CreateRequestUrl(string baseUrl, string hashSecret)
    {
        var queryString = BuildQueryString(_requestData);
        var secureHash = HmacSha512(hashSecret, queryString);
        return $"{baseUrl}?{queryString}&vnp_SecureHash={secureHash}";
    }

    /// <summary>Recomputes the signature over the response params and compares it (case-insensitive).</summary>
    public bool ValidateSignature(string inputHash, string hashSecret)
    {
        if (string.IsNullOrEmpty(inputHash))
            return false;

        var raw = BuildQueryString(_responseData);
        var computed = HmacSha512(hashSecret, raw);
        return computed.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }

    private static string BuildQueryString(SortedList<string, string> data)
    {
        var sb = new StringBuilder();
        foreach (var (key, value) in data)
            sb.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");

        if (sb.Length > 0)
            sb.Length--; // drop the trailing '&'

        return sb.ToString();
    }

    private static string HmacSha512(string key, string input)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder(hash.Length * 2);
        foreach (var b in hash)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}

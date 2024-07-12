using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace API.Shared.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Truncates string so that it is no longer than the specified number of characters.
        /// </summary>
        /// <param name="str">String to truncate.</param>
        /// <param name="length">Maximum string length.</param>
        /// <returns>Original string or a truncated one if the original was too long.</returns>
        public static string? Truncate(this string? str, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
            }

            if (str == null)
            {
                return null;
            }

            var maxLength = Math.Min(str.Length, length);
            return str[..maxLength];
        }

        public static bool IsEmpty([NotNullWhen(false)] this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNotEmpty([NotNullWhen(true)] this string? str)
        {
            return !str.IsEmpty();
        }

        public static bool ContainsAny(this string? str, params string[] values)
        {
            return str.IsNotEmpty() && values.Any(str.Contains);
        }

        public static bool ContainsAny(this string? str, params char[] values)
        {
            return str.IsNotEmpty() && values.Any(str.Contains);
        }

        /// <summary>
        /// Extension method to decode a Base64 string.
        /// </summary>
        /// <param name="base64EncodedData">The encoded data.</param>
        /// <returns>The decoded string.</returns>
        public static string DecodeBase64(this string base64EncodedData)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Extension method to Base64 encode a string.
        /// </summary>
        /// <param name="plainText">The text to encode</param>
        /// <returns>The encoded string.</returns>
        public static string EncodeBase64(this string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Extension method to decode a Base64URL string.
        /// </summary>
        /// <param name="base64UrlEncodedData">The encoded data.</param>
        /// <returns>The decoded string.</returns>
        public static string DecodeBase64Url(this string base64UrlEncodedData)
        {
            return Base64UrlEncoder.Decode(base64UrlEncodedData);
        }

        /// <summary>
        /// Extension method to Base64Url encode a string.
        /// </summary>
        /// <param name="plainText">The text to encode</param>
        /// <returns>The encoded string.</returns>
        public static string EncodeBase64Url(this string plainText)
        {
            return Base64UrlEncoder.Encode(plainText);
        }

        public static string Base64UrlEncodeObject(object obj)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
            return Base64UrlEncoder.Encode(plainTextBytes);
        }

        public static T Base64UrlDecodeObject<T>(string base64String)
        {
            var base64EncodedBytes = Base64UrlEncoder.DecodeBytes(base64String);
            return JsonSerializer.Deserialize<T>(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
        }
    }
}

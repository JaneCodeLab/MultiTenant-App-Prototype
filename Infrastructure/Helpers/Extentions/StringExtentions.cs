
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Infrastructure.Helpers
{
    public static class StringExtentions
    {
        public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value.Trim());

        public static int ToInt(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return 0;

            return Int32.Parse(source.Trim());
        }

        public static string Hash(this string source)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(source));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        public static string Replace(this string? value, Dictionary<string, object>? replacements = default)
        {
            if (value is null)
                return string.Empty;


            if (replacements is not null)
                foreach (var item in replacements)
                    value = value.Replace($"{{{item.Key}}}", item.Value?.ToString(), StringComparison.OrdinalIgnoreCase);
            return value!;
        }

        public static string GetFirstCharacters(this string source)
        {
            var result = "";
            var words = source.Split(" ");

            foreach (var item in words)
            {
                if (item.Length > 0)
                    result += item[0];
            }
            return result.ToUpper();
        }

        public static string ToUpperFirstChar(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
    }
}
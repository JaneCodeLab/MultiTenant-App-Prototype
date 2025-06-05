
using System.Globalization;

namespace Infrastructure.Helpers
{
    public static class DateTimeExtentions
    {
        public static TimeSpan? DiffFromNow(this DateTime? value)
        {
            if (value == null)
                return TimeSpan.Zero;

            return value - DateTime.Now.ToUniversalTime();
        }

        public static DateTime? GetLocalDateTime(this DateTime? timeUtc, string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId))
                return null;

            if (timeUtc == null || timeUtc == new DateTime())
                return null;

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            if (timeZone == null)
                return null;

            if (timeUtc == null || timeUtc == new DateTime())
                return null;
            else
                return TimeZoneInfo.ConvertTimeFromUtc((DateTime)timeUtc, timeZone);
        }

        public static DateTime GetLocalDateTime(this DateTime timeUtc, string timeZoneId)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc((DateTime)timeUtc, timeZone);
        }

        public static DateTime? GetUtcDateTime(this DateTime? timeUtc, string sourceTimeZoneId)
        {
            if (string.IsNullOrEmpty(sourceTimeZoneId))
                return null;

            var sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId);
            if (sourceTimeZone == null)
                return null;

            if (timeUtc == null || timeUtc == new DateTime())
                return null;

            return TimeZoneInfo.ConvertTimeToUtc((DateTime)timeUtc, sourceTimeZone);
        }

        public static DateTime GetUtcDateTime(this DateTime timeUtc, string sourceTimeZoneId)
        {
            var sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc((DateTime)timeUtc, sourceTimeZone);
        }

        public static string ToDateTimeStyle(this DateTime input) => input.ToString("dd/MM/yyyy HH:mm:ss");

        /// <summary>
        /// Converts string to DateTime.
        /// Input string should be in dd.MM.yyyy format
        /// </summary>
        /// <param name="source">Should be in dd.MM.yyyy format </param>
        /// <returns></returns>
        public static DateTime? ToDate(this string? source)
        {
            try
            {
                if (string.IsNullOrEmpty(source))
                    return null;

                var input = source.Replace("/", ".").Replace("-", ".").Split(' ')[0].Split('.');
                return new DateTime(input[2].ToInt(), input[1].ToInt(), input[0].ToInt());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetServerDateTimeCulture()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
        }

        public static string GetServerDateCulture()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        }
    }
}

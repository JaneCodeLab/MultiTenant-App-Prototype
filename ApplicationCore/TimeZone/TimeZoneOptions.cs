namespace ApplicationCore
{
    public static class TimeZoneOptions
    {
        public static List<TimeZoneModel> GetTimeZoneList()
        {
            var result = new List<TimeZoneModel>();
            foreach (var timezone in TimeZoneInfo.GetSystemTimeZones())
            {
                result.Add(new TimeZoneModel { Id = timezone.Id, Title = timezone.DisplayName });
            }
            return result;
        }
    }
}

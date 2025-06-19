namespace ApplicationService
{
    public static class JwtSettings
    {
        public const string Section = "Jwt";
        public const string Key = $"{Section}:Key";
        public const string Issuer = $"{Section}:Issuer";
        public const string Audience = $"{Section}:Audience";
    }
}

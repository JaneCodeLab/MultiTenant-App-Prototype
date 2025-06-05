namespace Infrastructure.Helpers
{
    public record FileDownload(byte[] FormFile, string ContentType, string fileName);
}

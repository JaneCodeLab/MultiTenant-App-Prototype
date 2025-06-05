
namespace Infrastructure.Helpers;

public class ApiMediaTypes
{
    public const string Pdf = "application/pdf";
    public const string Json = "application/json";
    public const string png = "application/png";
    public const string Jpg = "application/jpg";
    public const string Jpeg = "application/jpeg";
    public const string Text = "application/text";
    public const string JsonPatch = "application/json-patch+json";

    public static string GetFileType(string fileExtention)
    {
        if (fileExtention.ToLower() == "pdf")
            return Pdf;
        if (fileExtention.ToLower() == "json")
            return Json;
        if (fileExtention.ToLower() == "png")
            return png;
        if (fileExtention.ToLower() == "jpg")
            return Jpg;
        if (fileExtention.ToLower() == "jpeg")
            return Jpeg;
        if (fileExtention.ToLower() == "txt")
            return Text;

        return Text;
    }
}
namespace RpgLoadoutApp.Infrastructure;

public static class FileFormatExtensions
{
    public static string ToExtension(this FileFormat format)
    {
        return format switch
        {
            FileFormat.Txt => "txt",
            FileFormat.Json => "json",
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }
}
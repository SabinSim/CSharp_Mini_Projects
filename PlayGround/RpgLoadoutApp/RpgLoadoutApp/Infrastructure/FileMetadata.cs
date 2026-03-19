namespace RpgLoadoutApp.Infrastructure;

public class FileMetadata
{
    public string FileName { get; }
    public FileFormat Format { get; }

    public FileMetadata(string fileName, FileFormat format)
    {
        FileName = fileName;
        Format = format;
    }

    public string ToPath()
    {
        return $"{FileName}.{Format.ToExtension()}";
    }
}
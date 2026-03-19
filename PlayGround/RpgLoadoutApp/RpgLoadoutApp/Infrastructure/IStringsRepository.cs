namespace RpgLoadoutApp.Infrastructure;

public interface IStringsRepository
{
    List<string> Read(string path);
    void Write(string path, List<string> lines);
}
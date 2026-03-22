using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MusicPlaylistApp.Infrastructure;

public class StringsJsonRepository : IStringsRepository
{
    public List<string> Read(string path)
    {
        if (!File.Exists(path))
        {
            return new List<string>();
        }

        string json = File.ReadAllText(path);
        List<string>? lines = JsonSerializer.Deserialize<List<string>>(json);
        return lines ?? new List<string>();
    }

    public void Write(string path, List<string> lines)
    {
        string json = JsonSerializer.Serialize(lines);
        File.WriteAllText(path, json);
    }
}
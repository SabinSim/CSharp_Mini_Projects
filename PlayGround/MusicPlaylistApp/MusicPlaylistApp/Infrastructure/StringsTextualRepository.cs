using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicPlaylistApp.Infrastructure;

public class StringsTextualRepository : IStringsRepository
{
    public List<string> Read(string path)
    {
        if (!File.Exists(path))
        {
            return new List<string>();
        }

        return File.ReadAllLines(path).ToList();
    }

    public void Write(string path, List<string> lines)
    {
        File.WriteAllLines(path, lines);
    }
}
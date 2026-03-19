# RpgLoadoutApp

## 📁 폴더 구조

```
RpgLoadoutApp/
├── Program.cs
├── Infrastructure/
│   ├── FileFormat.cs
│   ├── FileFormatExtensions.cs
│   ├── FileMetadata.cs
│   ├── IStringsRepository.cs
│   ├── StringsJsonRepository.cs
│   └── StringsTextualRepository.cs
├── Items/
│   ├── IItemRepository.cs
│   ├── Item.cs
│   └── ItemRepository.cs
└── Loadouts/
    ├── Loadout.cs
    └── LoadoutRepository.cs
```

---

## Program.cs

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using RpgLoadoutApp.Infrastructure;
using RpgLoadoutApp.Items;
using RpgLoadoutApp.Loadouts;

namespace RpgLoadoutApp;

class Program
{
    private const FileFormat CurrentFileFormat = FileFormat.Json;

    static void Main()
    {
        try
        {
            if (System.IO.File.Exists("player_loadout.json"))
                System.IO.File.Delete("player_loadout.json");

            FileMetadata fileMetadata = new FileMetadata("player_loadout", CurrentFileFormat);

            IStringsRepository stringsRepository = CurrentFileFormat == FileFormat.Json
                ? new StringsJsonRepository()
                : new StringsTextualRepository();

            IItemRepository itemRepository = new ItemRepository();

            LoadoutRepository loadoutRepository =
                new LoadoutRepository(stringsRepository, itemRepository, fileMetadata);

            Loadout warriorSet = new Loadout(new List<int> { 1, 2, 3 });
            Loadout mageSet = new Loadout(new List<int> { 4, 5, 6 });

            loadoutRepository.Write(warriorSet);
            loadoutRepository.Write(mageSet);

            List<Loadout> savedLoadouts = loadoutRepository.Read();

            Console.WriteLine("[savedLoadouts] completed loadouts:");

            int setNumber = 1;
            foreach (Loadout loadout in savedLoadouts)
            {
                var itemNames = loadout.ItemIds
                    .Select(id => itemRepository.GetById(id).Name);

                Console.WriteLine($"Set {setNumber++}: {string.Join(", ", itemNames)}");
            }

            Console.Out.Flush();
            Console.WriteLine("\n아무 키나 누르면 종료됩니다...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[에러 발생] {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
        }
    }
}
```

---

## Infrastructure/FileFormat.cs

```csharp
namespace RpgLoadoutApp.Infrastructure;

public enum FileFormat
{
    Txt,
    Json
}
```

---

## Infrastructure/FileFormatExtensions.cs

```csharp
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
```

---

## Infrastructure/FileMetadata.cs

```csharp
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
```

---

## Infrastructure/IStringsRepository.cs

```csharp
namespace RpgLoadoutApp.Infrastructure;

public interface IStringsRepository
{
    List<string> Read(string path);
    void Write(string path, List<string> lines);
}
```

---

## Infrastructure/StringsJsonRepository.cs

```csharp
using System.IO;
using System.Text.Json;

namespace RpgLoadoutApp.Infrastructure;

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
```

---

## Infrastructure/StringsTextualRepository.cs

```csharp
using System.IO;

namespace RpgLoadoutApp.Infrastructure;

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
```

---

## Items/IItemRepository.cs

```csharp
namespace RpgLoadoutApp.Items;

public interface IItemRepository
{
    Item GetById(int id);
}
```

---

## Items/Item.cs

```csharp
namespace RpgLoadoutApp.Items;

public class Item
{
    public int Id { get; }
    public string Name { get; }

    public Item(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
```

---

## Items/ItemRepository.cs

```csharp
using System.Collections.Generic;
using System.Linq;

namespace RpgLoadoutApp.Items;

public class ItemRepository : IItemRepository
{
    private readonly List<Item> _items = new List<Item>
    {
        new Item(1, "Excalibur"),
        new Item(2, "Dragon Shield"),
        new Item(3, "Health Potion"),
        new Item(4, "Archmage Staff"),
        new Item(5, "Mana Ring"),
        new Item(6, "Teleport Scroll")
    };

    public Item GetById(int id)
    {
        Item item = _items.SingleOrDefault(x => x.Id == id);
        if (item == null)
        {
            throw new Exception($"\n[error] Item with id {id} not found.");
        }
        return item;
    }
}
```

---

## Loadouts/Loadout.cs

```csharp
using System.Collections.Generic;

namespace RpgLoadoutApp.Loadouts;

public class Loadout
{
    public List<int> ItemIds { get; }

    public Loadout(List<int> itemIds)
    {
        ItemIds = itemIds;
    }
}
```

---

## Loadouts/LoadoutRepository.cs

```csharp
using System.Collections.Generic;
using System.Linq;
using RpgLoadoutApp.Infrastructure;
using RpgLoadoutApp.Items;

namespace RpgLoadoutApp.Loadouts;

public class LoadoutRepository
{
    private const string Separator = ",";

    private readonly IStringsRepository _stringsRepository;
    private readonly IItemRepository _itemRepository;
    private readonly FileMetadata _fileMetadata;

    public LoadoutRepository(
        IStringsRepository stringsRepository,
        IItemRepository itemRepository,
        FileMetadata fileMetadata)
    {
        _stringsRepository = stringsRepository;
        _itemRepository = itemRepository;
        _fileMetadata = fileMetadata;
    }

    public void Write(Loadout loadout)
    {
        List<string> existingLines = _stringsRepository.Read(_fileMetadata.ToPath());
        string line = string.Join(Separator, loadout.ItemIds);
        existingLines.Add(line);
        _stringsRepository.Write(_fileMetadata.ToPath(), existingLines);
    }

    public List<Loadout> Read()
    {
        List<string> lines = _stringsRepository.Read(_fileMetadata.ToPath());
        List<Loadout> loadouts = new List<Loadout>();

        foreach (string line in lines)
        {
            loadouts.Add(CreateLoadoutFromLine(line));
        }

        return loadouts;
    }

    private Loadout CreateLoadoutFromLine(string line)
    {
        List<int> itemIds = line
            .Split(Separator)
            .Select(int.Parse)
            .ToList();

        foreach (int id in itemIds)
        {
            _itemRepository.GetById(id);
        }

        return new Loadout(itemIds);
    }
}
```


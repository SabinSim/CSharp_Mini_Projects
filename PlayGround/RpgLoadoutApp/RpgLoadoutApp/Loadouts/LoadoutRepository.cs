using System.Collections.Generic;
using System.Linq;
using RpgLoadoutApp.Infrastructure;
using RpgLoadoutApp.Items;

namespace RpgLoadoutApp.Loadouts;

public class LoadoutRepository
{
    // const는 쉽게 말해 "변하지 않는 값"을 의미합니다. 즉, 프로그램이 실행되는 동안 이 값은 변경될 수 없습니다.
    // const로 선언된 변수는 컴파일 타임에 상수로 평가되며, 런타임에 변경할 수 없습니다. 따라서 const로 선언된 변수는 프로그램 전체에서 동일한 값을 유지합니다.
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
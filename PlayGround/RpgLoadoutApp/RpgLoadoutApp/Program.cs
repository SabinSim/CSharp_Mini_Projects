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
        if (System.IO.File.Exists("player_loadouts.json"))
            System.IO.File.Delete("player_loadouts.json");
        
        FileMetadata fileMetadata = new FileMetadata("player_loadout",CurrentFileFormat);

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

        Console.ReadKey();
    }
}
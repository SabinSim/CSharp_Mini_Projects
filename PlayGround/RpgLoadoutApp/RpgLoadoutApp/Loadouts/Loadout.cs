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
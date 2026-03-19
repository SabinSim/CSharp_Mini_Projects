using System;
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
        new Item(5, "Mana Ring")
    };

    public Item GetbyId(int id)
    {
        Item item = _items.SingleOrDefault(x => x.Id == id);
        if (item == null)
        {
            throw new Exception($"\n[오류] 데이터베이스에 {id}번")
        }
    }
}
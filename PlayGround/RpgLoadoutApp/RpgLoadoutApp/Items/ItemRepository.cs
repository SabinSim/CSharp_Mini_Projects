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

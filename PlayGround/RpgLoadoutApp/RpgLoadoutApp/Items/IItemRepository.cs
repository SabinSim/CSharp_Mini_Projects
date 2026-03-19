namespace RpgLoadoutApp.Items;

public interface IItemRepository
{
    Item GetById(int id);
}
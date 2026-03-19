namespace RpgLoadoutApp.Items;

public interface IItemRepository
{
    Item GetbyId(int id);
}
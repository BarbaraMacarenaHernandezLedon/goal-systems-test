using System.Collections.Generic;

namespace inventario.Item.Models
{
    public interface IItemRepository
    {
        string Add(ItemModel item);
        ItemModel Find(string itemId);
        string IsValid(ItemModel item);
        ItemModel Remove(string itemId);
        List<ItemModel> GetAll();
    }
}

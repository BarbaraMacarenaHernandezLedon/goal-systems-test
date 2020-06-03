using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace inventario.Item.Models
{
    public class ItemRepository : IItemRepository
    {
        // Create a "ConcurrentDictionary <ItemId, ItemModel>" structure to store the items in memory.
        private static ConcurrentDictionary<string, ItemModel> _items = new ConcurrentDictionary<string, ItemModel>();

        // Definition of item methods.
        public string Add(ItemModel item)
        {
            item.Id = Guid.NewGuid().ToString();
            _items[item.Id] = item;

            return item.Id;
        }

        public ItemModel Find(string itemId)
        {
            ItemModel item;
            _items.TryGetValue(itemId, out item);
            return item;
        }

        // By now this method only verifies if the expiration date is correct, if it is 
        // not, it throws an error message.
        public string IsValid(ItemModel item)
        {
            if (item.ExpirationDate <= System.DateTime.Now) 
            {
                return "El elemento ya se encuentra caducado";
            }

            return null;
        }

        public ItemModel Remove(string itemId)
        {
            ItemModel item;
            bool isDeleted = _items.TryRemove(itemId, out item);

            if (!isDeleted)
            {
                return null;
            }

            return item;
        }

        public List<ItemModel> GetAll()
        {
            return _items.Values.ToList();
        }
    }
}

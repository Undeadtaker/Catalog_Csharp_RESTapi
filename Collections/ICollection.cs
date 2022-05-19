using Catalog.Entities;

namespace Catalog.Collections
{
    public interface IInMemoryCollections
    {
        Item GetItem(Guid this_id);
        IEnumerable<Item> GetItems();
        void CreateItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(Guid id);
    }
}
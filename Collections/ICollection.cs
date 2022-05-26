using Catalog.Entities;

namespace Catalog.Collections
{
    public interface IInMemoryCollections
    {
        Task<Item> GetItemAsync(Guid this_id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid this_id);
    }
}
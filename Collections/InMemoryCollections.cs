using Catalog.Entities;

namespace Catalog.Collections
{

    public class InMemoryCollections : IInMemoryCollections
    {

        // Variables of Class, non static, so unique to every InMemoryCollections object
        private readonly List<Item> items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion",         Price = 9,  CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Iron Sword",     Price = 20, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Bronze Shield",  Price = 18, CreatedDate = DateTimeOffset.UtcNow }
        };


        // Methods
        public IEnumerable<Item> GetItems() { return items; }
        public Item GetItem(Guid this_id) { return items.Where(item => item.Id == this_id).SingleOrDefault(); }
        public void CreateItem(Item item) { items.Add(item); }

        public void UpdateItem(Item item) 
        {
            var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
            items[index] = item;
        }

        public void DeleteItem(Guid id) 
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
        }

    }
}
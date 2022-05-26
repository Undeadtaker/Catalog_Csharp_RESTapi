using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Collections
{
    public class MongoDBCollections : IInMemoryCollections
    {
        
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsColletion;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter; 

        public MongoDBCollections(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsColletion = database.GetCollection<Item>(collectionName);
        }

        // Create Item async 
        public async Task CreateItemAsync(Item item) { await itemsColletion.InsertOneAsync(item); }

        // Get all Items
        public async Task<IEnumerable<Item>> GetItemsAsync(){ return await itemsColletion.Find(new BsonDocument()).ToListAsync(); }

        // Delete an Item
        public async Task DeleteItemAsync(Guid this_id)
        {
            var filter = filterBuilder.Eq(item => item.Id, this_id);
            await itemsColletion.DeleteOneAsync(filter);
        }

        // Get an Item 
        public async Task<Item> GetItemAsync(Guid this_id)
        {
            var filter = filterBuilder.Eq(item => item.Id, this_id);
            return await itemsColletion.Find(filter).SingleOrDefaultAsync();
        }

        // Update an Item
        public async Task UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await itemsColletion.ReplaceOneAsync(filter, item);
        }
    }
}


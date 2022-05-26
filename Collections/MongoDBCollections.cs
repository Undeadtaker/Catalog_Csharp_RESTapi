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

        // Create Item
        public void CreateItem(Item item) { itemsColletion.InsertOne(item); }

        // Get all Items
        public IEnumerable<Item> GetItems(){ return itemsColletion.Find(new BsonDocument()).ToList(); }

        // Delete an Item
        public void DeleteItem(Guid this_id)
        {
            var filter = filterBuilder.Eq(item => item.Id, this_id);
            itemsColletion.DeleteOne(filter);
        }

        // Get an Item 
        public Item GetItem(Guid this_id)
        {
            var filter = filterBuilder.Eq(item => item.Id, this_id);
            return itemsColletion.Find(filter).SingleOrDefault();
        }

        // Update an Item
        public void UpdateItem(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            itemsColletion.ReplaceOne(filter, item);
        }
    }
}


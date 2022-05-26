namespace Catalog.Config
{
    public class MongoDBConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public string ConnectionString 
        { 
            get
            {
                return $"mongodb://{Host}:{Port}";
            } 
        }
    }
}
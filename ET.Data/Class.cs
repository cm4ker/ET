using LiteDB;

namespace ET.Data
{
    public class Class
    {
        public const string CollectionName = "classes";
        
        [BsonId] public int Id { get; set; }

        public string Name { get; set; }
    }
}
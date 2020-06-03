using LiteDB;

namespace ET.Data
{
    public class Word
    {
        public const string CollectionName = "words";
        
        [BsonId] public int Id { get; set; }
        public string Name { get; set; }
    }
}
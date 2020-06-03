using LiteDB;

namespace ET.Data
{
    public class Meaning
    {
        public const string CollectionName = "meanings";
        
        [BsonId] public int Id { get; set; }

        public string Value { get; set; }

        [BsonRef(Language.CollectionName)] public Language Language { get; set; }
    }
}
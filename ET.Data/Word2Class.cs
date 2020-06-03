using LiteDB;

namespace ET.Data
{
    public class Word2Class
    {
        public const string CollectionName = "word2classes";
        
        [BsonRef("words")] public Word Word { get; set; }

        [BsonRef("classes")] public Class Class { get; set; }
    }
}
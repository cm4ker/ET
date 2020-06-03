using LiteDB;

namespace ET.Data
{
    public class Comment
    {
        public const string CollectionName = "comment";
        [BsonId] public int Id { get; set; }

        public Meaning Meaning { get; set; }
    }
}
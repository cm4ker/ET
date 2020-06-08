using LiteDB;

namespace ET.Data
{
    public class Translation
    {
        public const string CollectionName = "translations";

        [BsonId] public int Id { get; set; }

        [BsonRef(Language.CollectionName)] public Language LanguageFrom { get; set; }
        [BsonRef(Language.CollectionName)] public Language LanguageTo { get; set; }

        [BsonRef(Word.CollectionName)] public Word Word { get; set; }

        [BsonRef(Topic.CollectionName)] public Topic Topic { get; set; }

        [BsonRef(Meaning.CollectionName)] public Meaning Meaning { get; set; }
    }
}
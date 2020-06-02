using System.Collections.Generic;
using LiteDB;

namespace ET.Data
{
    public class Topic
    {
        [BsonId] public int Id { get; set; }

        public string ShortName { get; set; }
        public string Description { get; set; }
    }

    public class Class
    {
        [BsonId] public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Word
    {
        [BsonId] public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Word2Class
    {
        [BsonRef("words")] public Word Word { get; set; }

        [BsonRef("classes")] public Class Class { get; set; }
    }

    public class Meaning
    {
        [BsonId] public int Id { get; set; }

        public string Value { get; set; }

        [BsonRef("languages")] public Language Language { get; set; }
    }

    public class Comment
    {
        [BsonId] public int Id { get; set; }

        public Meaning Meaning { get; set; }
    }

    public class Translation
    {
        [BsonId] public int Id { get; set; }

        [BsonRef("languages")]
        public Language Language { get; set; }
        
        [BsonRef("words")]
        public Word Word { get; set; }
        
        [BsonRef("topics")]
        public Topic Topic { get; set; }
        
        [BsonRef("meanings")]
        public Meaning Meaning { get; set; }
    }
}
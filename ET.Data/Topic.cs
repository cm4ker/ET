using System.Collections.Generic;
using LiteDB;

namespace ET.Data
{
    public class Topic
    {
        public const string CollectionName = "topics";
        
        [BsonId] public int Id { get; set; }

        public string ShortName { get; set; }
        public string Description { get; set; }
    }
}
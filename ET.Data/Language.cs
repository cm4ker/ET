using System;
using LiteDB;

namespace ET.Data
{
    public class Language
    {
        public const string CollectionName = "languages";
        
        public int Id { get; set; }

        public string Name { get; set; }
        
    }
}
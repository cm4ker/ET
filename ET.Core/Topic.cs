using System.Collections.Generic;

namespace ET.Core
{
    public class Topic
    {
        public string ShortName { get; set; }
        public string Description { get; set; }
        public List<Meaning> Meanings { get; set; }
    }
}
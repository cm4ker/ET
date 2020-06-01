using System.Collections.Generic;

namespace ET.Core
{
    public class Translation
    {
        public TranslationHeader Header { get; set; }
        public List<Topic> Topic { get; set; }
    }
}
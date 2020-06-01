using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks.Dataflow;
using ET.Core;

namespace ET.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Parser();
            var translations = a.Translate(args[0]);

            foreach (var trans in translations)
            {
                Console.WriteLine($"{trans.Header.Text} ({trans.Header.WordClass})");

                foreach (var topic in trans.Topic)
                {
                    Console.WriteLine($"\t{topic.ShortName}({topic.Description})");
                    foreach (var meaning in topic.Meanings)
                    {
                        Console.Write($"\t\t {meaning.Text}");
                        if (meaning.Comment != null)
                            Console.Write($"{meaning.Comment.Text} author: {meaning.Comment.Author}");

                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
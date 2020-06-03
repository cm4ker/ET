using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks.Dataflow;
using ET.Core;
using ET.Data;
using Topic = ET.Data.Topic;

namespace ET.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Parser();
            var translations = a.Translate(args[0]);

            DataContext dc = new DataContext("test.db");

            var cl = new Class() {Name = "hello"};
            var wd = new Word {Name = "SomeWord"};
            var lang = new Language {Name = "ru"};
            var mean = new Data.Meaning {Language = lang, Value = "SomeMeaning"};
            var t = new Data.Topic {Description = "some desc", ShortName = "short name"};

            var trns = new Data.Translation {Language = lang, Meaning = mean, Topic = t, Word = wd};

            dc.Classes.Insert(cl);
            dc.Words.Insert(wd);
            dc.Languages.Insert(lang);
            dc.Meanings.Insert(mean);
            dc.Topics.Insert(t);
            dc.Translations.Insert(trns);

            var b = dc.Translations.Query()
                .Include(x => x.Meaning)
                .Include(x => x.Word)
                .ToList();

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
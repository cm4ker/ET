using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks.Dataflow;
using ET.Core;
using ET.Data;
using Meaning = ET.Data.Meaning;
using Topic = ET.Data.Topic;
using Translation = ET.Core.Translation;

namespace ET.Desktop
{
    public class TranslationPrinter
    {
        public static string Print(Translation trans)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{trans.Header.Word} ({trans.Header.WordClass})");

            foreach (var topic in trans.Topic)
            {
                sb.AppendLine($"\t{topic.ShortName}({topic.Description})");
                foreach (var meaning in topic.Meanings)
                {
                    sb.Append($"\t\t {meaning.Text}");
                    if (meaning.Comment != null)
                        sb.Append($"{meaning.Comment.Text} author: {meaning.Comment.Author}");

                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }

    class Program
    {
        public static void StoreTranslation(DataContext dc, Translation trans, Topic topic, Core.Meaning meaning)
        {
            var cl = dc.Classes.FindOne(x => x.Name == trans.Header.WordClass);

            if (cl == null)
            {
                cl = new Class {Name = trans.Header.WordClass};
                dc.Classes.Insert(cl);
            }

            var langEn = dc.Languages.FindOne(x => x.Name == "en");
            var langRu = dc.Languages.FindOne(x => x.Name == "ru");


            if (langEn == null)
            {
                langEn = new Language {Name = "en"};
                dc.Languages.Insert(langEn);
            }

            if (langRu == null)
            {
                langRu = new Language {Name = "ru"};
                dc.Languages.Insert(langRu);
            }

            var word = dc.Words.FindOne(x => x.Name == trans.Header.Word);
            if (word == null)
            {
                word = new Word {Name = trans.Header.Word};
                dc.Words.Insert(word);
            }

            var mean = dc.Meanings.FindOne(x => x.Value == meaning.Text);

            if (mean == null)
            {
                mean = new Meaning {Value = meaning.Text, Language = langRu};
                dc.Meanings.Insert(mean);
            }

            var dbTopic = dc.Topics.FindOne(x => x.Description == topic.Description && x.ShortName == topic.ShortName);
            if (dbTopic == null)
            {
                dbTopic = new Topic {Description = topic.Description, ShortName = topic.ShortName};
                dc.Topics.Insert(dbTopic);
            }

            var trns = dc.Translations.FindOne(x =>
                x.LanguageFrom == langEn && x.LanguageTo == langRu && x.Meaning == mean && x.Topic == topic);

            if (trns == null)
            {
                trns = new Data.Translation
                    {Meaning = mean, Topic = dbTopic, Word = word, LanguageFrom = langEn, LanguageTo = langRu};
                dc.Translations.Insert(trns);
            }
        }

        static void Main(string[] args)
        {
            DataContext dc = new DataContext("test.db");

            var a = new Parser();
            var translations = a.Translate(args[0]);

            foreach (var translation in translations)
            {
                Console.WriteLine(TranslationPrinter.Print(translation));
            }
        }
    }
}
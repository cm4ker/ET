using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Nito.Collections;

namespace ET.Core
{
    public class Parser
    {
        private readonly Dictionary<string, int> _languageCodes = new Dictionary<string, int>
        {
            {"en", 1}, {"ru", 2}
        };

        private const string Url = "http://www.multitran.ru/c/m.exe";
        private const string TablePath = ".//div[@class='middle_col']/table[1]";

        private IEnumerable<Translation> parse_translation_page(string pageContent)
        {
            var rows = new Deque<HtmlNode>(get_all_rows(pageContent));

            while (rows.Any() && is_separator(rows.RemoveFromFront()) && !is_unknown_separator(rows))
            {
                yield return ParseTranslation(rows);
            }
        }

        private static HtmlNodeCollection get_all_rows(string content)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var table = doc.DocumentNode.SelectSingleNode(TablePath);
            return table.SelectNodes("tr");
        }

        private static bool is_separator(HtmlNode row)
        {
            return row.SelectSingleNode("td[@class]") is null;
        }

        private bool is_unknown_separator(Deque<HtmlNode> rows)
        {
            var sep = rows.RemoveFromFront();

            if (sep.SelectSingleNode("td[@colspan]") != null && sep.SelectSingleNode("td[@class]") == null)
                return true;

            rows.AddToFront(sep);
            return false;
        }

        private string LoadPage(string phrase)
        {
            UriBuilder ub = new UriBuilder(Url)
            {
                Query = $"s={phrase}&l1={_languageCodes["en"]}&l2={_languageCodes["ru"]}"
            };

            var wc = new WebClient();
            var content = wc.DownloadString(ub.Uri);

            return content;
        }

        public List<Translation> Translate(string phrase)
        {
            return new List<Translation>(parse_translation_page(LoadPage(phrase)));
        }


        private Translation ParseTranslation(Deque<HtmlNode> rows)
        {
            return new Translation
            {
                Header = ParseHeader(rows.RemoveFromFront()),
                Topic = ParseTopics(rows)
            };
        }

        private Header ParseHeader(HtmlNode row)
        {
            var translationHeaderElement = row.SelectSingleNode("td[@class='gray']");

            if (translationHeaderElement == null) throw new Exception("header is null");

            var header = new Header
            {
                Word = translationHeaderElement.SelectSingleNode("a").InnerText
            };


            var isPrefix = true;

            foreach (var element in translationHeaderElement.ChildNodes)
            {
                if (element.Name == "a")
                {
                    isPrefix = false;
                    continue;
                }

                if (element.Name == "span")
                {
                    if (isPrefix)
                        header.WordPrefix = element.InnerText;

                    isPrefix = false;
                    continue;
                }

                if (element.Name == "span")
                {
                    if (isPrefix)
                    {
                        header.WordPrefix = element.InnerText;
                        isPrefix = false;
                        continue;
                    }

                    if (element.GetAttributeValue("style", "") == "color:gray")
                    {
                        header.Pronunciation = element.InnerText;
                        continue;
                    }

                    if (element.GetAttributeValue("class", "") == "small")
                    {
                        continue;
                    }
                }

                if (element.Name == "em")
                {
                    header.WordClass = element.InnerText;
                }
            }

            return header;
        }

        private List<Topic> ParseTopics(Deque<HtmlNode> rows)
        {
            List<Topic> topics = new List<Topic>();

            while (rows.Any())
            {
                var row = rows.RemoveFromFront();

                if (is_separator(row))
                {
                    rows.AddToFront(row);
                    break;
                }

                topics.Add(ParseTopic(row));
            }

            return topics;
        }

        private Topic ParseTopic(HtmlNode row)
        {
            HtmlNode
                topicElement = row.SelectSingleNode("td[@class='subj']"),
                meaningsElement = row.SelectSingleNode("td[@class='trans']");

            var topicLink = topicElement.SelectSingleNode("a");


            return new Topic
            {
                ShortName = topicLink?.InnerText,
                Description = topicLink?.GetAttributeValue("title", ""),
                Meanings = ParseMeanings(meaningsElement).ToList()
            };
        }


        private IEnumerable<Meaning> ParseMeanings(HtmlNode meaningsElement)
        {
            var result = new Meaning();

            foreach (var element in meaningsElement.ChildNodes)
            {
                if (element.Name == "a")
                {
                    result.Text = element.InnerText;
                }
                else if (element.Name == "span")
                {
                    var commentText = element.InnerText;

                    if (commentText == "(")
                        commentText = "";

                    var author = "";
                    var authorElement = element.SelectSingleNode("i/a");

                    if (authorElement != null)
                        author = authorElement.InnerText;

                    result.Comment = new Comment {Author = author, Text = commentText};
                }

                if (element.InnerText.Trim().EndsWith(';'))
                {
                    yield return result;
                    result = new Meaning();
                }
            }

            yield return result;
        }
    }
}
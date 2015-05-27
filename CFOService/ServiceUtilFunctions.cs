using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFOService
{
    public class ServiceUtilFunctions
    {
        public SimpleWODObject GetWOD()
        {
            string Url = "http://www.crossfitorlando.com/category/wod/#/today";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(Url);
            IEnumerable<HtmlNode> articles = document.DocumentNode.SelectNodes("//article");

            var list = ServiceUtilFunctions.GetSimpleList(articles);

            return list[0];
        }

        static public List<SimpleWODObject> GetSimpleList(IEnumerable<HtmlNode> articles)
        {
            var list = new List<SimpleWODObject>();
            
            foreach(HtmlNode article in articles)
            {   
                SimpleWODObject obj = new SimpleWODObject() {
                   title = article.SelectSingleNode("//h2/a").InnerText,
                   body = ServiceUtilFunctions.FormatBody(article.SelectSingleNode("//article//div[2]").InnerText)
                };

                list.Add(obj);
            }

            return list;
        }

        static public string FormatBody(string body)
        {
            // remove the expected 3 tabs in the begining of the string and replace the Part 1 and Part 2 of the WOD with additional new lines...
            return body.Replace("\n\t\t\t", "").Replace("\nPart ", "\n\nPart ");
        }
    }
}

public class SimpleWODObject
{
    public string title;

    public string body;
}
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using System.Text.RegularExpressions;

namespace WebJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static void ScrapeAndStoreWOD([Table("WOD")] ICollector<WOD> tableBinding)
        {
            WOD wod = Functions.Scape();

            // TODO: have Store method live up to its name; the Add method call should reside in it.
            Functions.Store(wod);
            tableBinding.Add(wod);
        }

        static public WOD Scape()
        {
            string Url = "http://www.crossfitorlando.com/category/wod/#/today";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(Url);
            IEnumerable<HtmlNode> articles = document.DocumentNode.SelectNodes("//article");

            var list = Functions.GetSimpleList(articles);

            return list[0];
        }

        static public WOD Store(WOD wod)
        {
            string date = Regex.Match(wod.Title, @"\d+[-.\/]\d+[-.\/]\d+", RegexOptions.None).Value;
            DateTime dt = Convert.ToDateTime(date);

            wod.PartitionKey = "year_" + dt.Year.ToString();
            wod.RowKey = "day_" + dt.DayOfYear.ToString();

            //tableBinding.Add(wod);
            return wod;
        }

        static public List<WOD> GetSimpleList(IEnumerable<HtmlNode> articles)
        {
            var list = new List<WOD>();
            
            foreach(HtmlNode article in articles)
            {   
                WOD obj = new WOD() {
                   Title = article.SelectSingleNode("//h2/a").InnerText,
                   Body = Functions.FormatBody(article.SelectSingleNode("//article//div[2]").InnerText)
                };

                list.Add(obj);
            }

            return list;
        }

        static public string FormatBody(string body)
        {
            // one-offs format fixes...
            body.Replace("&#8230;", "...");
            body.Replace("&nbsp;", " ");

            // remove the expected 3 tabs in the begining of the string and replace the Part 1 and Part 2 of the WOD with additional new lines...
            return body.Replace("\n\t\t\t", "").Replace("\nPart ", "\n\nPart ");
        }
    }
}
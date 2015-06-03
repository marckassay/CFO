using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

            Functions.Store(tableBinding, wod);
        }

        static public WOD Scape()
        {
            string Url = "http://www.crossfitorlando.com/category/wod/#/today";

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(Url);
            HtmlNode article = document.DocumentNode.SelectNodes("//article").First();

            return ConvertHTMLToObject(article);
        }

        static public void Store(ICollector<WOD> tableBinding, WOD wod)
        {
            string date = Regex.Match(wod.Title, @"\d+[-.\/]\d+[-.\/]\d+", RegexOptions.None).Value;
            DateTime dt = Convert.ToDateTime(date);

            wod.PartitionKey = "year_" + dt.Year.ToString();
            wod.RowKey = "day_" + dt.DayOfYear.ToString();

            tableBinding.Add(wod);
        }

        static public WOD ConvertHTMLToObject(HtmlNode article)
        {
            WOD obj = new WOD() {
                Title = article.SelectSingleNode("//h2/a").InnerText,
                Body = FormatBody(article.SelectSingleNode("//article//div[2]").InnerText)
            };

            return obj;
        }

        static public string FormatBody(string body)
        {
            // attempt to remove all html entities, tabs and newlines...
            try
            {
               return HttpUtility.HtmlDecode(body).Trim().Replace("Part ", "\nPart ");
            }
            // if this is caught, then at least return something useful...
            catch(Exception e)
            {
                return body;
            }
        }
    }
}
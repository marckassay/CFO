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
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage;

namespace WebJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static void ScrapeAndStoreWOD([Table("WOD")] ICollector<WOD> tableBinding)
        {
            WOD wod = ConvertArticleToObject(Scape("http://www.crossfitorlando.com/category/wod/#/today").First());

            Functions.Store(tableBinding, wod);
        }

        [NoAutomaticTrigger]
        public static void ScrapeAndStoreWODs([Table("WOD")] ICollector<WOD> tableBinding)
        {
           for (int i = 1; i < 30; i++)
           {
                string url = (i == 1)? "http://www.crossfitorlando.com/category/wod/#/today" : String.Format("http://www.crossfitorlando.com/category/wod/page/{0}/#/today", i.ToString());
                
                List<WOD> wods = ConvertArticlesToObjects(Scape(url));

                wods.ForEach(delegate(WOD wod)
                {
                    // one-offs WOD creates a poison message.  need to read up on handling it: https://msdn.microsoft.com/en-us/library/ms789028%28v=vs.110%29.aspx
                    if (wod.Title == "Friday the 13th!")
                    {
                        wod.Title = "Friday 11/13/15";
                        Console.WriteLine(wod.Title);
                        Functions.Store(tableBinding, wod);
                    }
                    else if(wod.Title == "Friday 7/23/15")
                    {
                        wod.Title = "Friday 7/24/15";
                        Console.WriteLine(wod.Title);
                        Functions.Store(tableBinding, wod);
                    }
                    else if (wod.Title == "Tuesday 7/13/15")
                    {
                        wod.Title = "Tuesday 7/14/15";
                        Console.WriteLine(wod.Title);
                        Functions.Store(tableBinding, wod);
                    }
                    else if (wod.Title == "Thursday 3/12/15")
                    {
                        Console.WriteLine("***"+wod.Title);
                    }
                    else if (wod.Title != "Saturday 2/7/15 Pre-class WU")
                    {
                        Console.WriteLine(wod.Title);
                        Functions.Store(tableBinding, wod);
                    }
                });
            }
        }

        static public IEnumerable<HtmlNode> Scape(string url)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument document = web.Load(url);

            return document.DocumentNode.SelectNodes("//article");
        }

        static public void Store(ICollector<WOD> tableBinding, WOD wod)
        {
            string date = Regex.Match(wod.Title, @"\d+[-.\/]\d+[-.\/]\d+", RegexOptions.None).Value;

            // there was at least one occurance of title WOD not having a date...
            if (date == "")
                return;

            DateTime dt = Convert.ToDateTime(date);

            wod.PartitionKey = "year_" + dt.Year.ToString();
            wod.RowKey = dt.DayOfYear.ToString();

            try
            {
                tableBinding.Add(wod);
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
               
                // swallow exception; this occurs when the entity already exists
            }
        }

        static public WOD ConvertArticleToObject(HtmlNode article)
        {
            WOD obj = new WOD() {
                Title = article.SelectSingleNode(".//h2[@class='post-title']/a").InnerText,
                Body = FormatBody(article.SelectSingleNode(".//div[@class='entry clearfix']").InnerText)
            };

            return obj;
        }

        static public List<WOD> ConvertArticlesToObjects(IEnumerable<HtmlNode> nodes)
        {
            var list = new List<WOD>();

            foreach (HtmlNode node in nodes)
            {
                list.Add(ConvertArticleToObject(node));
            }

            return list;
        }

        static public string FormatBody(string body)
        {
            // attempt to remove all html entities, tabs and newlines...
            try
            {
               return HttpUtility.HtmlDecode(body).Trim().Replace("Part ", "\nPart ");
            }
            // if this is caught, then at least return something useful...
            catch(Exception)
            {
                return body;
            }
        }
    }
}
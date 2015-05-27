using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CFOService
{
    public class Service : IService
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
    }
}

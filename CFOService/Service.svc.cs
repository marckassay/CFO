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
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

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

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using WebJob;

namespace ServiceWebRole
{
    public class Service : IService
    {
        /// <summary>
        /// Returns a WOD (Workout of the Day) object or objects.  This data comes from http://www.crossfitorlando.com/.
        /// </summary>
        /// <param name="DateEx">An expression to retrieve archive WODs.  
        /// For an example: GetWOD("4/25/15"); // returns a WOD for April 25th 2015
        /// Another example: GetWOD("4/25/15 -2"); // returns WOD objects for 4/23-4/25  
        /// Another example: GetWOD("4/13/15 +2"); // returns WOD objects for 4/25-4/27  
        /// Another example: GetWOD("*"); // returns a random WOD object 
        /// </param>
        /// <returns>If DateEx is empty, a WOD object with a Title and Body property.  If DateEx has a valid expression, it can return multiple object of WOD type.</returns>
        public IEnumerable<WOD> GetWOD(string DateEx = "")
        {
            if(DateEx == "")
            {
                IEnumerable<WOD> today = QueryAndExecuteTable(DateTime.Today, null);

                // see if DateTime.Today returns a WOD, if not return yesterday's WOD.
                if (today.First().IsEmpty == false)
                    return today;
                
                //...returns yesterday's WOD...
                return QueryAndExecuteTable(DateTime.Today.AddDays(-1), null);
            }
            else if(DateEx == "*")
            {
                IEnumerable<WOD> random = null;

                sbyte attempts = 1;

                while (attempts < 11)
                {
                    random = QueryAndExecuteTable(GetRandomDay(), null);

                    if ((random.First().IsEmpty == false) || (attempts == 10))
                        break;

                    attempts++;
                }

                return random;
            }
            else
            {
                // split DateEx between the space that may be between the date and range...
                string[] dateAndRange = Regex.Split(DateEx, @"\s");

                string[] pattern = { "M-d-yyyy", "M/d/yyyy", "M-d-yy", "M/d/yy" };
        
                DateTime parsedDate;

                // see if the date part of dateAndRange is conformed correctly...
                if (DateTime.TryParseExact(dateAndRange[0], pattern, null, DateTimeStyles.None, out parsedDate))
                {
                    if(dateAndRange.Length == 1)
                    {
                        return QueryAndExecuteTable(parsedDate, null);
                    }
                    else if(dateAndRange.Length == 2)
                    {
                        return QueryAndExecuteTable(parsedDate, sbyte.Parse(dateAndRange[1]));
                    }
                    else
                    {
                        throw new Exception("The format of DateEx is incorrect.  Have DateEx conformed as the following examples: 4/25/15 , 4/25/15 -2 , 4/13/15 +2");
                    }
                }
                else
                {
                    throw new Exception("The format of DateEx is incorrect.  Have DateEx conformed as the following examples: 4/25/15 , 4/25/15 -2 , 4/13/15 +2");
                }
            }
        }

        private IEnumerable<WOD> QueryAndExecuteTable(DateTime date, sbyte? range)
        {
            CloudTable table = GetCloudTable("cfo.WOD");
            
            sbyte rangeDefined = (range.HasValue == false) ? sbyte.Parse("0") : range.Value;

            List<Keys> keys = GetPartitionAndRowKeys(date, rangeDefined);

            List<WOD> results = new List<WOD>();

            foreach (var key in keys)
            {
                // TODO: need to query using GenerateFilterConditionForInt on the RowKey.  this will return mulitple Rows.
                string partitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key.Partition);
                string rowFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, key.Row.ToString());

                // Construct the query operation to retrieve a WOD entity matching partition and row key...
                TableQuery<WOD> query = new TableQuery<WOD>().Where(
                    TableQuery.CombineFilters(partitionFilter, TableOperators.And, rowFilter)
                    );

                try
                {
                    results.Add(table.ExecuteQuery(query).First<WOD>());  
                }
                catch (InvalidOperationException)
                {

                    results.Add(new WOD()
                    {
                        // TODO: convert key.Row to Date so that it can be used in the following title...
                        Title = "There was no WOD for this day.",
                        IsEmpty = true
                    });
                }
                
            };

            return results;
        }

        private CloudTable GetCloudTable (string tableUri)
        {
            // split tableUri on the period ...
            string[] accountAndName = Regex.Split(tableUri, @"\.");

            // Retrieve the storage account from the connection string.
            string account = CloudConfigurationManager.GetSetting(accountAndName[0]);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(account);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "WOD" table.
            CloudTable table = tableClient.GetTableReference(accountAndName[1]);

            return table;
        }

        private List<Keys> GetPartitionAndRowKeys(DateTime date, sbyte range)
        {
            var results = new List<Keys>();

            while (range != 0)
            {
                if(range > 0)
                {
                    results.Add(GetKey(date.AddDays(range)));

                    range--;
                }
                else if(range < 0)
                {
                    results.Add(GetKey(date.AddDays(range)));

                    range++;
                }
            }

            results.Add(GetKey(date));

            return results;
        }

        private Keys GetKey(DateTime date)
        {
           return new Keys()
           {
                Partition = "year_" + date.Year,
                Row =  date.DayOfYear
           };
        }

        private DateTime GetRandomDay()
        {
            DateTime start = new DateTime(2014, 12, 16);
            Random gen = new Random();

            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}

class Keys
{
    public string Partition { get; set; }
    public int Row { get; set; }
}
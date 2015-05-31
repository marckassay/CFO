using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WebJob;

namespace ServiceWebRole
{
    public class Service1 : IService1
    {
        public string GetWOD()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("cfo"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("WOD");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<WOD> query = new TableQuery<WOD>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "year_2015"));

            // Print the fields for each customer.
            foreach (WOD entity in table.ExecuteQuery(query))
            {
                return entity.Title + " " + entity.Body;
            }

            return "nil";
        }
    }
}

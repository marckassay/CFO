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
    public class Service : IService
    {
        public WOD GetWOD()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cfo"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "WOD" table.
            CloudTable table = tableClient.GetTableReference("WOD");

            // Construct the query operation for all WOD entities where PartitionKey="year_2015".
            TableQuery<WOD> query = new TableQuery<WOD>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "year_2015"));

            return table.ExecuteQuery(query).Last<WOD>();
        }
    }
}

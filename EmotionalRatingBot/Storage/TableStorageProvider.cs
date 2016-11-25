using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace EmotionalRatingBot.Storage
{
    public class TableStorageProvider
    {
        public void SaveData(SelfieData selfieData)
        {
            CloudStorageAccount storageAccount = ConfigurationProvider.CreateCloudStorageAccount();
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("SelfieData");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            TableOperation insertOperation = TableOperation.Insert(selfieData);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }
    }
}
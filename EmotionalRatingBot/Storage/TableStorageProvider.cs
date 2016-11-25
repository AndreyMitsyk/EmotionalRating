using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using EmotionalRatingBot.CognitiveServices;

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

        public ChartData GetChartData()
        {

            var testData = new ChartData();
            EmotionData[] emotionData = new EmotionData[] {
                new EmotionData { Name = Emotions.emotion.Anger.ToString(), Count = 1 },
                new EmotionData { Name = Emotions.emotion.Contempt.ToString(), Count = 2 },
                new EmotionData { Name = Emotions.emotion.Disgust.ToString(), Count = 0 },
                new EmotionData { Name = Emotions.emotion.Fear.ToString(), Count = 1 },
                new EmotionData { Name = Emotions.emotion.Happiness.ToString(), Count = 5 },
                new EmotionData { Name = Emotions.emotion.Neutral.ToString(), Count = 5 },
                new EmotionData { Name = Emotions.emotion.Sadness.ToString(), Count = 2 },
                new EmotionData { Name = Emotions.emotion.Surprise.ToString(), Count = 4 }
            };

            testData.Emotions = emotionData;
            testData.PrimaryRating = 90;
            testData.Sex = 60;
            return testData;
        }
    }
}
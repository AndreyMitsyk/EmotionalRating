using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using EmotionalRatingBot.CognitiveServices;
using System.Collections.Generic;

namespace EmotionalRatingBot.Storage
{
    public class TableStorageProvider
    {
        private const string EventName = "HAKVELON";
        // Create the table client.
        private CloudTableClient tableClient = ConfigurationProvider.CreateCloudStorageAccount().CreateCloudTableClient();

        public void SaveData(SelfieData selfieData)
        {
            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("SelfieData");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();
            selfieData.Event = EventName;

            TableOperation insertOperation = TableOperation.Insert(selfieData);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public ChartData GetChartData()
        {
            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("SelfieData");
            TableQuery<SelfieData> query = new TableQuery<SelfieData>().Where(TableQuery.GenerateFilterCondition("Event", QueryComparisons.Equal, EventName));
            var chartData = new ChartData();
            Dictionary<string, int> emotionData = new Dictionary<string, int>();
            int maleCount = 0;
            int totalCount = 0;
            var tableData = table.ExecuteQuery(query);

            foreach (SelfieData entity in tableData)
            {
                string emotionName = entity.RowKey;
                if (emotionData.ContainsKey(emotionName))
                {
                    emotionData[emotionName] = emotionData[emotionName] + 1;
                } else
                {
                    emotionData.Add(emotionName, 1);
                }
                if (entity.Sex == "male")
                {
                    maleCount++;
                }
                totalCount++;
            }

            if (totalCount != 0)
            {
                chartData.Sex = maleCount / totalCount * 100;
            } else
            {
                chartData.Sex = 50;
            }

            int goodEmotions = 0;
            int allEmotions = 0;

            foreach(var emotion in emotionData)
            {
                if (emotion.Key == Emotions.emotion.Happiness.ToString() || emotion.Key == Emotions.emotion.Surprise.ToString() || emotion.Key == Emotions.emotion.Neutral.ToString())
                {
                    goodEmotions += emotion.Value;
                }
                allEmotions += emotion.Value;
            }
            chartData.PrimaryRating = goodEmotions / allEmotions * 100;

            return chartData;
        }
    }
}
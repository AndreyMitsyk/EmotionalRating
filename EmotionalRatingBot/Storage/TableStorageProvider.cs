using Microsoft.WindowsAzure.Storage.Table;
using EmotionalRatingBot.CognitiveServices;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using System;

namespace EmotionalRatingBot.Storage
{
    public class TableStorageProvider
    {
        private const string EventName = "HAKVELON";
        // Create the table client.
        private CloudTableClient tableClient = ConfigurationProvider.CreateCloudStorageAccount().CreateCloudTableClient();

        public void SaveData(Emotions emotion, Face face, string url)
        {
            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("SelfieData");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();
            SelfieData selfieData = new SelfieData(url, emotion.EmotionName, emotion.EmotionValue, face.FaceAttributes.Gender);
            selfieData.Event = EventName;

            TableOperation insertOperation = TableOperation.InsertOrReplace(selfieData);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public ChartData GetChartData()
        {
            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("SelfieData");
            TableQuery<SelfieData> query = new TableQuery<SelfieData>().Where(TableQuery.GenerateFilterCondition("Event", QueryComparisons.Equal, EventName));
            var chartData = new ChartData();
            Dictionary<string, EmotionData> emotionData = new Dictionary<string, EmotionData>();
            int maleCount = 0;
            int totalCount = 0;
            var tableData = table.ExecuteQuery(query);
            if (tableData == null || !tableData.Any())
            {
                return null;
            }
            foreach (var entity in tableData)
            {
                string emotionName = entity.PartitionKey;
                if (emotionData.ContainsKey(emotionName))
                {
                    emotionData[emotionName].Value = emotionData[emotionName].Value + 1;
                    if (float.Parse(entity.EmotionValue) > emotionData[emotionName].MaxValue)
                    {
                        emotionData[emotionName].MaxValue = float.Parse(entity.EmotionValue);
                        emotionData[emotionName].Url = entity.ImageUrl;
                    }
                } else
                {
                    emotionData.Add(emotionName, new EmotionData(1, entity.ImageUrl, float.Parse(entity.EmotionValue)));
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

            foreach(var emotion in emotionData)
            {
                if (emotion.Key == Emotions.emotion.Happiness.ToString() || emotion.Key == Emotions.emotion.Surprise.ToString() || emotion.Key == Emotions.emotion.Neutral.ToString())
                {
                    goodEmotions += emotion.Value.Value;
                }
            }
            chartData.PrimaryRating = goodEmotions / totalCount * 100;
            chartData.Emotions = emotionData;

            return chartData;
        }
    }
}
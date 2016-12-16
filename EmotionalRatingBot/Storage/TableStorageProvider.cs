using Microsoft.WindowsAzure.Storage.Table;
using EmotionalRatingBot.CognitiveServices;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ProjectOxford.Face.Contract;
using System.Globalization;
using System;

namespace EmotionalRatingBot.Storage
{
    public class TableStorageProvider
    {
        private const string EventName = "Meetup";
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
            DataManager.GetInstance().Update();
        }

        public ChartData GetChartData()
        {
            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("SelfieData");
            TableQuery<SelfieData> query = new TableQuery<SelfieData>().Where(TableQuery.GenerateFilterCondition("Event", QueryComparisons.Equal, EventName));
            var chartData = new ChartData();
            Dictionary<string, EmotionData> emotionData = new Dictionary<string, EmotionData>();
            IList<string> lastPhotos = new List<string>();

            int maleCount = 0;
            int totalCount = 0;
            var tableData = table.ExecuteQuery(query).OrderByDescending(r => r.Timestamp);
            if (tableData == null || !tableData.Any())
            {
                return null;
            }
            foreach (var entity in tableData)
            {
                if (lastPhotos.Count <= 5)
                {
                    if (!lastPhotos.Any(u => u == entity.ImageUrl))
                    {
                        lastPhotos.Add(entity.ImageUrl);
                    }
                }

                string emotionName = entity.PartitionKey;
                if (emotionData.ContainsKey(emotionName))
                {
                    emotionData[emotionName].Value = emotionData[emotionName].Value + 1;
                    if (float.Parse(entity.EmotionValue, CultureInfo.InvariantCulture) > emotionData[emotionName].MaxValue)
                    {
                        emotionData[emotionName].MaxValue = float.Parse(entity.EmotionValue, CultureInfo.InvariantCulture);
                        emotionData[emotionName].Url = entity.ImageUrl;
                    }
                } else
                {
                    emotionData.Add(emotionName, new EmotionData(1, entity.ImageUrl, float.Parse(entity.EmotionValue, CultureInfo.InvariantCulture)));
                }
                if (entity.Sex == "male")
                {
                    maleCount++;
                }
                totalCount++;
            }

            if (totalCount != 0)
            {
                chartData.Sex = (float)maleCount / (float)totalCount * 100;
            } else
            {
                chartData.Sex = 50;
            }

            float goodEmotions = 0;

            foreach(var emotion in emotionData)
            {
                if (emotion.Key == Emotions.emotion.Happiness.ToString())
                {
                    goodEmotions += emotion.Value.Value;
                } else if (emotion.Key == Emotions.emotion.Surprise.ToString())
                {
                    goodEmotions += (float)(emotion.Value.Value * 0.6);
                } else if(emotion.Key == Emotions.emotion.Neutral.ToString())
                {
                    goodEmotions += (float)(emotion.Value.Value * 0.25);
                }
            }
            chartData.LastPhotos = lastPhotos;
            chartData.PrimaryRating = (int)(goodEmotions / (float)totalCount * 100);
            chartData.Emotions = emotionData;

            return chartData;
        }
    }
}
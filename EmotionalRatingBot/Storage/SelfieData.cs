using Microsoft.WindowsAzure.Storage.Table;
using EmotionalRatingBot.CognitiveServices;
using System;

namespace EmotionalRatingBot.Storage
{
    public class SelfieData: TableEntity
    {
        public SelfieData(string imageUrl, Emotions.emotion emotion, float emotionValue, string sex)
        {
            this.PartitionKey = emotion.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            this.ImageUrl = imageUrl;
            this.EmotionValue = emotionValue.ToString();
            this.Sex = sex;
        }

        public SelfieData() { }

        public string ImageUrl { get; set; }

        public string EmotionValue { get; set; }

        public string Sex { get; set; }

        public string Event { get; set; }
    }
}
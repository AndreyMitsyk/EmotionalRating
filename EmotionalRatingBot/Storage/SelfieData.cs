using Microsoft.WindowsAzure.Storage.Table;
using EmotionalRatingBot.CognitiveServices;
using System;

namespace EmotionalRatingBot.Storage
{
    public class SelfieData: TableEntity
    {
        public SelfieData(string imageUrl, Emotions.emotion emotion, float emotionValue, int age, string sex)
        {
            this.PartitionKey = imageUrl;
            this.RowKey = emotion.ToString();
            this.EmotionValue = emotionValue;
            this.Age = age;
            this.Sex = sex;
        }

        public SelfieData() { }

        public float EmotionValue { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public string Event { get; set; }
    }
}
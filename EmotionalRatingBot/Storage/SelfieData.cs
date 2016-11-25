using Microsoft.WindowsAzure.Storage.Table;
using EmotionalRatingBot.CognitiveServices;

namespace EmotionalRatingBot.Storage
{
    public class SelfieData: TableEntity
    {
        public enum SexValues { Male, Female };

        public SelfieData(string imageUrl, Emotions.emotion emotion, float emotionValue)
        {
            this.PartitionKey = imageUrl;
            this.RowKey = emotion.ToString();
            this.EmotionValue = emotionValue;
        }

        public SelfieData() { }

        public float EmotionValue { get; set; }

        public SexValues Sex { get; set; }

        public int Age { get; set; }
    }
}
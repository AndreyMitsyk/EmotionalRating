using Microsoft.WindowsAzure.Storage.Table;
using EmotionalRatingBot.CognitiveServices;

namespace EmotionalRatingBot.Storage
{
    public class SelfieData: TableEntity
    {
        public SelfieData(string imageUrl, Emotions.emotion emotion)
        {
            this.PartitionKey = imageUrl;
            this.RowKey = emotion.ToString();
        }

        public SelfieData() { }

        public string Sex { get; set; }

        public string Age { get; set; }
    }
}
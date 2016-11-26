namespace EmotionalRatingBot.Storage
{
    public class EmotionData
    {
        public EmotionData(int value, string url, float maxValue)
        {
            this.Value = value;
            this.Url = url;
            this.MaxValue = maxValue;
        }

        public int Value { get; set; }

        public string Url { get; set; }

        public float MaxValue { get; set; }
    }
}
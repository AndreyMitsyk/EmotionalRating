﻿namespace EmotionalRatingBot.CognitiveServices
{
    public class Emotions
    {
        public enum emotion { Anger, Contempt, Disgust, Fear, Happiness, Neutral, Sadness, Surprise, Test }

        public emotion EmotionName { get; set; }

        public float EmotionValue { get; set; }
    }
}
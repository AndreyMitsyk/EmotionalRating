using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion;
using System;

namespace EmotionalRatingBot.CognitiveServices
{
    public class EmotionProvider
    {
        public async Task<Microsoft.ProjectOxford.Emotion.Contract.Emotion[]> GetEmotions(string imageUrl)
        {
            // TODO: replace sample API key
            string OxfordAPIKey = "2cabd9f1b2014a04bc04782b3c703539";
            EmotionServiceClient Oxford = new EmotionServiceClient(OxfordAPIKey);

            var results = await Oxford.RecognizeAsync(imageUrl);

            if (results != null && results.Length > 0)
            {
                return results;
            }
            return null;
        }

        public Emotions.emotion GetMaxEmotion(Microsoft.ProjectOxford.Emotion.Contract.Emotion personEmotion)
        {
            var emotions = personEmotion.Scores.ToRankedList();
            float maxValue = 0;
            Emotions.emotion maxEmotion = Emotions.emotion.Happiness;
            foreach (var emotion in emotions)
            {
                if (emotion.Value > maxValue)
                {
                    maxValue = emotion.Value;
                    maxEmotion = (Emotions.emotion)Enum.Parse(typeof(Emotions.emotion), emotion.Key);
                }
            }
            return maxEmotion;
        }
    }
}
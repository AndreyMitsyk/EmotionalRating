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

        public Emotions GetMaxEmotion(Microsoft.ProjectOxford.Emotion.Contract.Emotion personEmotion)
        {
            var emotions = personEmotion.Scores.ToRankedList();

            Emotions maxEmotion = new Emotions {
                EmotionName = Emotions.emotion.Happiness,
                EmotionValue = 0
            };
            foreach (var emotion in emotions)
            {
                if (emotion.Value > maxEmotion.EmotionValue)
                {
                    maxEmotion.EmotionValue = emotion.Value;
                    maxEmotion.EmotionName = (Emotions.emotion)Enum.Parse(typeof(Emotions.emotion), emotion.Key);
                }
            }
            return maxEmotion;
        }
    }
}
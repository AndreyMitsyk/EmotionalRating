using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion;

namespace EmotionalRatingBot.Emotions
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
    }
}
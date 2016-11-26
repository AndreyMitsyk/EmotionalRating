namespace EmotionalRatingBot.ImageCore
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;

    using EmotionalRatingBot.CognitiveServices;

    using Microsoft.ProjectOxford.Emotion.Contract;

    public class SmileStickerFactory
    {


        private Dictionary<Emotions.emotion, Bitmap> images;
        public SmileStickerFactory()
        {
            this.images = new Dictionary<Emotions.emotion, Bitmap>()
                              {
                                  { Emotions.emotion.Anger, Resource1.angry },
                                  { Emotions.emotion.Contempt, Resource1.contempt },
                                  { Emotions.emotion.Disgust, Resource1.disgust },
                                  { Emotions.emotion.Fear, Resource1.fear },
                                  { Emotions.emotion.Happiness, Resource1.happiness },
                                  { Emotions.emotion.Neutral, Resource1.neutral },
                                  { Emotions.emotion.Sadness, Resource1.sadness },
                                  { Emotions.emotion.Surprise, Resource1.surprise },
                                  { Emotions.emotion.Test, Resource1.happiness }
                              };
        }

        public Image GetSticker(Emotions.emotion emotionType)
        {
            return this.images[emotionType];
        }
    }
}
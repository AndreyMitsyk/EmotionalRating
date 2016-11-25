namespace EmotionalRatingBot.Common
{
    using System.Drawing;
    using Microsoft.ProjectOxford.Emotion;
    using Microsoft.ProjectOxford.Emotion.Contract;

    public interface IProcessingOrder
    {
        Image GetUserImage();
        Emotion[] GetEmotion();
        IProcessingCallback GetCallback();
    }
}
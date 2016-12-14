namespace EmotionalRatingBot.ImageCore
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;

    using CognitiveServices;
    using Storage;

    using Microsoft.ProjectOxford.Emotion.Contract;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.ProjectOxford.Face.Contract;
    using System.Collections.Generic;

    public class ImageProcessingService
    {
        private EmotionProvider emotionProvider;
        private TableStorageProvider tableStorageProvider;

        private ImageReactor reactor;

        #region Init
        public static ImageProcessingService GetService()
        {
            return new ImageProcessingService();
        }

        private ImageProcessingService()
        {
            this.emotionProvider = new EmotionProvider();
            this.tableStorageProvider = new TableStorageProvider();
            this.reactor = new ImageReactor();
        }
        #endregion

        public async Task<string> Process(CloudBlockBlob imageBlob, Face[] faces)
        {
            Emotion[] emotions = await this.emotionProvider.GetEmotions(imageBlob.Uri.AbsoluteUri);
            var emotionResults = this.UpdateImage(imageBlob, emotions, faces);
            var result = "Максимальные эмоции для фотографии: ";

            foreach (var emotion in emotionResults)
            {
                result += $"{emotion}; ";
            }

            return result;
        }

        private List<string> UpdateImage(CloudBlockBlob imageBlob, Emotion[] emotions, Face[] faces)
        {
            var image = this.LoadImage(imageBlob.Uri.AbsoluteUri);
            var emotionValues = new List<string>();
            int i = 0;
            foreach (var emotion in emotions)
            {
                var maxEmotion = emotionProvider.GetMaxEmotion(emotion);
                var rectangle = new Rectangle(
                    emotion.FaceRectangle.Left,
                    emotion.FaceRectangle.Top,
                    emotion.FaceRectangle.Width,
                    emotion.FaceRectangle.Height);
                reactor.Decorate(image, rectangle, maxEmotion.EmotionName);
                if (maxEmotion.EmotionValue == 1)
                {
                    maxEmotion.EmotionValue = (float)0.9999999;
                }
                emotionValues.Add($"{this.GetLocalizedEmotionName(maxEmotion.EmotionName.ToString())}: {maxEmotion.EmotionValue}");
                tableStorageProvider.SaveData(maxEmotion, faces[i], imageBlob.Uri.AbsoluteUri);

                i++;
            }
            SaveImage(imageBlob, image);

            return emotionValues;
        }

        private string GetLocalizedEmotionName(string emotionName)
        {
            var emotions = new Dictionary<string, string> {
                { "anger", "Гнев" },
                { "contempt", "Презрение" },
                { "disgust", "Отвращение" },
                { "fear", "Страх" },
                { "happiness", "Счастье" },
                { "neutral", "Нейтральный" },
                { "sadness", "Грусть" },
                { "surprise", "Удивление" }
            };
            return emotions[emotionName.ToLower()];
        }

        private Image LoadImage(string uri)
        {
             return Image.FromStream(BlobStorageProvider.GetStreamFromUrl(uri));
        }

        private void SaveImage(CloudBlockBlob imageBlob, Image image)
        {
            BlobStorageProvider blobProvider = new BlobStorageProvider();
            using (Stream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;
                blobProvider.UpdateBlob(imageBlob, stream);
            }
        }

    }
}
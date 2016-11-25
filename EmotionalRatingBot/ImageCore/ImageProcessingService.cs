namespace EmotionalRatingBot.ImageCore
{
    using System;
    using System.Data.Services.Client;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;

    using CognitiveServices;
    using Storage;

    using Microsoft.ProjectOxford.Emotion.Contract;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class ImageProcessingService
    {
        private EmotionProvider emotionProvider;

        private ImageReactor reactor;

        #region Init
        public static ImageProcessingService GetService()
        {
            return new ImageProcessingService();
        }

        private ImageProcessingService()
        {
            this.emotionProvider = new EmotionProvider();
            this.reactor = new ImageReactor();
        }
        #endregion

        public async Task Process(CloudBlockBlob imageBlob)
        {
            Emotion[] emotions = await this.emotionProvider.GetEmotions(imageBlob.Uri.AbsoluteUri);
            this.UpdateImage(imageBlob, emotions);
        }

        private void UpdateImage(CloudBlockBlob imageBlob, Emotion[] emotions)
        {
            var image = this.LoadImage(imageBlob.Uri.AbsoluteUri);
            foreach (var emotion in emotions)
            {
                var rectangle = new Rectangle(
                    emotion.FaceRectangle.Left,
                    emotion.FaceRectangle.Top,
                    emotion.FaceRectangle.Width,
                    emotion.FaceRectangle.Height);
                reactor.Decorate(image, rectangle, emotionProvider.GetMaxEmotion(emotion));
            }
            SaveImage(imageBlob, image);
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
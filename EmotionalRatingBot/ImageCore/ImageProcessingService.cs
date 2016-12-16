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
    using System;

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
                { "neutral", "Без эмоций" },
                { "sadness", "Грусть" },
                { "surprise", "Удивление" }
            };
            return emotions[emotionName.ToLower()];
        }

        private Image LoadImage(string uri)
        {
            // Fix for the IOS images
            var img = Image.FromStream(BlobStorageProvider.GetStreamFromUrl(uri));
            if (Array.IndexOf(img.PropertyIdList, 274) > -1)
            {
                var orientation = (int)img.GetPropertyItem(274).Value[0];
                switch (orientation)
                {
                    case 1:
                        // No rotation required.
                        break;
                    case 2:
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        img.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        img.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                // This EXIF data is now invalid and should be removed.
                img.RemovePropertyItem(274);
            }
            return img;
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
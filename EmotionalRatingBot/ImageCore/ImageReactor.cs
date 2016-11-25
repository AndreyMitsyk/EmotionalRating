using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmotionalRatingBot.ImageCore
{
    using System.Drawing;

    using EmotionalRatingBot.CognitiveServices;

    public class ImageReactor
    {
        private SmileStickerFactory smileStickerFactory;

        private const float SMILE_DEFAULT_SCALE_RATIO = 0.1f;

        private const float BORDER_WIDTH_RATIO = 0.005f;

        private Color penColor;

        private int borderSize;

        public ImageReactor()
        {
            this.smileStickerFactory = new SmileStickerFactory();
            this.penColor = Color.FromArgb(200, Color.Green);
        }

        public void Decorate(Image sourceImage, Rectangle rect, Emotions.emotion emotionType)
        {
            this.borderSize = (int) ((sourceImage.Width + sourceImage.Height) * BORDER_WIDTH_RATIO);
            if (this.borderSize % 2 != 0)
            {
                this.borderSize++;
            }
            
            this.AddRect(sourceImage, rect);
            this.AddSmile(sourceImage, rect, emotionType);
        }

        private void AddSmile(Image sourceImage, Rectangle rect, Emotions.emotion emotionType)
        {
            var ratio = (int)(Math.Min(sourceImage.Width, sourceImage.Height) * SMILE_DEFAULT_SCALE_RATIO);
            using (Graphics g = Graphics.FromImage(sourceImage))
            {
                g.DrawImage(
                    this.smileStickerFactory.GetSticker(emotionType),
                    rect.Right - (ratio / 2),
                    rect.Bottom - (ratio / 2),
                    ratio,
                    ratio);
            }
        }

        private void AddRect(Image sourceImage, Rectangle rect)
        {
            
            using (Graphics g = Graphics.FromImage(sourceImage))
            {
                g.DrawLines(new Pen(this.penColor, this.borderSize),  this.GetBorderPoints(rect));
            }
        }

        private Point[] GetBorderPoints(Rectangle rect)
        {
            return new[]
                       {
                           new Point(rect.Left, rect.Top), new Point(rect.Right, rect.Top),
                           new Point(rect.Right, rect.Bottom), new Point(rect.Left, rect.Bottom),
                           new Point(rect.Left, rect.Top - this.borderSize/2),
                       };
        }
    }
}
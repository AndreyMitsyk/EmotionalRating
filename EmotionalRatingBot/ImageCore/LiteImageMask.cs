using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalRatingBot.ImageCore
{
    using System.Drawing;
    using System.Drawing.Imaging;

    class LiteImageMask
    {
        private Bitmap mask;
        public LiteImageMask(Image sourceImage)
        {
            this.mask = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(this.mask))
            {
                g.Clear(Color.Transparent);
            }
        }

    }
}

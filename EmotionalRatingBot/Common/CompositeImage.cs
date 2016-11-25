namespace EmotionalRatingBot.Common
{
    using System.Drawing;

    public class CompositeImage
    {
        /// <summary>
        /// Изображение для отправки 
        /// </summary>
        public Image LiteCompositeImage { get; }

        /// <summary>
        /// Исходное изображение.
        /// </summary>
        public Image SourceUserImage { get; }

        /// <summary>
        /// Данные для отрисовки динамическиз фреймов.
        /// </summary>
        public CompositeImageMetadata Metadata { get; }

        
    }
}
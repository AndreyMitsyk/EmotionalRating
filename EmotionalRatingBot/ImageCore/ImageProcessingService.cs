namespace EmotionalRatingBot.ImageCore
{
    using System;

    using EmotionalRatingBot.Common;

    public class ImageProcessingService
    {
        #region Init
        private static ImageProcessingService service;
        private static Object mux = new Object();

        public static ImageProcessingService getInstance(string name)
        {
            if (service == null)
            {
                lock (mux)
                {
                    if (service == null)
                        service = new ImageProcessingService();
                }
            }
            return service;
        }

        private ImageProcessingService()
        {
            
        }
        #endregion

        public void Process(IProcessingOrder processingOrder)
        {
            
        }
    }
}
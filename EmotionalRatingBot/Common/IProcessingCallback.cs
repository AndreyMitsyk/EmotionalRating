namespace EmotionalRatingBot.Common
{
    public interface IProcessingCallback
    {
        /// <summary>
        /// Обработчик результата.
        /// </summary>
        /// <param name="result">Результат обработки изображения. Содержит все необходимое для отправки пользователю, сохранению в БД и т.д.</param>
        void Execute(CompositeImage result);
    }
}

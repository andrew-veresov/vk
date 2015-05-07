namespace VkNet.Utils
{
    /// <summary>
    /// Интерфейс обработчика, распознающего капчу.
    /// </summary>
    public interface ICaptchaSolver
    {
        /// <summary>
        /// Распознает текст капчи.
        /// </summary>
        /// <param name="url">Ссылка на изображение капчи.</param>
        /// <returns>Строка, содержащая текст, который был закодирован в капче.</returns>
        string Solve(string url);

        /// <summary>
        /// Сообщает, что последняя капча была распознана неверно.
        /// </summary>
        void CaptchaIsFalse();
    }
}
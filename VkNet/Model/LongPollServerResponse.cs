namespace VkNet.Model
{
    using VkNet.Utils;

    /// <summary>
    /// Объект, с помощью которого можно подключиться к серверу быстрых сообщений для мгновенного 
    /// получения приходящих сообщений и других событий.  
    /// См. описание <see href="http://vk.com/dev/messages.getLongPollServer"/>.
    /// </summary>
    public class LongPollServerResponse
    {
        /// <summary>
        /// Ключ для подключения.
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// Имя сервера быстрых сообщений.
        /// </summary>
        public string Server { get; internal set; }

        /// <summary>
        /// Отметка времени.
        /// </summary>
        public long Ts { get; internal set; }

        #region Методы

        internal static LongPollServerResponse FromJson(VkResponse response)
        {
            var longPollServerResponse = new LongPollServerResponse();

            longPollServerResponse.Key = response["key"];
            longPollServerResponse.Server = response["server"];
            longPollServerResponse.Ts = response["ts"];

            return longPollServerResponse;
        }

        #endregion
    }
}
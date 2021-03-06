﻿namespace VkNet.Utils
{
    using Enums.Filters;

    /// <summary>
    /// Интерфейс браузера, с помощью которого осуществляется сетевое взаимодействие.
    /// Интерфейс введен с целью обеспечения возможности выполнения модульного тестирования.
    /// </summary>
    public interface IBrowser
    {
        /// <summary>
        /// Выполняет JSON-запрос к ВКонтакте.
        /// </summary>
        /// <param name="url">URL, в котором закодированы параметры запроса.</param>
        /// <returns>Результат выполнения запроса, полученный от сервера в формате JSON.</returns>
        string GetJson(string url);

#if false
        /// <summary>
        /// Выполняет асинхронный JSON-запрос к ВКонтакте.
        /// </summary>
        /// <param name="url">URL, в котором закодированы параметры запроса.</param>
        /// <returns></returns>
        Task<string> GetJsonAsync(string url);
#endif

        /// <summary>
        /// Выполняет авторизацию ВКонтакте. 
        /// Если приложение с идентификатором <paramref name="appId"/> не было установлено у пользователя, имеющего 
        /// логин <paramref name="email"/>, то производится его установка с разрешениями <paramref name="settings"/>.
        /// </summary>
        /// <param name="appId">Идентификатор авторизуемого приложения.</param>
        /// <param name="email">Логин пользователя ВКонтакте (его почта).</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="settings">Запрашиваемые для работы разрешения.</param>
        /// <param name="captcha_sid">Индентификатор капчи</param>
        /// <param name="captcha_key">Текст капчи.</param>
        /// <returns>
        /// Информация об авторизации приложения.
        /// </returns>
        VkAuthorization Authorize(int appId, string email, string password, Settings settings, long? captcha_sid, string captcha_key);
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы с сообщениями.
    /// </summary>
    public class MessagesCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly MessagesCategory _messages;

        internal MessagesCategoryExtended(MessagesCategory messagesCategory, VkApi vk)
        {
            _messages = messagesCategory;
            _vk = vk;
        }

        /// <summary>
        /// Возвращает список всех входящих либо всех исходящих личных сообщений текущего пользователя. 
        /// </summary>
        /// <param name="type">Тип сообщений которые необходимо получить.
        /// Необходимо передать <see cref="MessageType.Received"/> для полученных сообщений и <see cref="MessageType.Sended"/>
        /// для отправленных пользователем сообщений.
        /// </param>
        /// <param name="totalCount">Общее количество сообщений, удовлетворяющих условиям фильтрации.</param>
        /// <param name="timeOffset">Максимальное время, прошедшее с момента отправки сообщения до текущего момента в секундах. 0, если Вы хотите получить сообщения любой давности.</param>
        /// <param name="filter">Фильтр возвращаемых сообщений.</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение. 
        /// Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются). 
        /// Обратите внимание что сообщения обрезаются по словам.</param>
        /// <param name="lastMessageId">Идентификатор сообщения, полученного перед тем, которое нужно вернуть последним (при условии, что после него было получено не более count сообщений, иначе необходимо использовать с параметром offset).</param>
        /// <returns>Список сообщений, удовлетворяющий условиям фильтрации.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/messages.get"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.28")]
        public ReadOnlyCollection<Message> GetAll(
            MessageType type,
            out int totalCount,
            TimeSpan? timeOffset = null,
            MessagesFilter? filter = null,
            int? previewLength = null,
            long? lastMessageId = null)
        {
            const int count = 200;
            var i = 0;
            var result = new List<Message>();

            do
            {
                var currentItems = _messages.Get(type, out totalCount, count, i * count, timeOffset, filter, previewLength, lastMessageId);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }


        /// <summary>
        /// Возвращает историю всех сообщений текущего пользователя с указанным пользователя или групповой беседы. 
        /// </summary>
        /// <param name="id">
        /// Если параметр <paramref name="isChat"/> равен false, то задает идентификатор пользователя, историю переписки 
        /// с которым необходимо вернуть.
        /// Если параметр <paramref name="isChat"/> равен true, то задает идентификатор беседы, историю переписки в которой 
        /// необходимо вернуть.
        /// </param>
        /// <param name="isChat">Признак нужно ли вернуть историю сообщений для беседы (true) или для указанного пользователя.</param>
        /// <param name="totalCount">Общее количество сообщений в истории.</param>
        /// <param name="inReverse">
        /// Если данный параметр равен true, то сообщения возвращаются в хронологическом порядке. 
        /// Если данный параметр равен false (по умолчанию), сообщения возвращаются в обратном хронологическом порядке. 
        /// </param>
        /// <param name="startMessageId">Идентификатор сообщения, начиная с которго необходимо получить последующие сообщения.</param>
        /// <returns>
        /// Запрошенные сообщения.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/messages.getHistory"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.21")]
        public ReadOnlyCollection<Message> GetAllHistory(
            long id,
            bool isChat,
            out int totalCount,
            bool? inReverse = null,
            long? startMessageId = null)
        {
            const int count = 200;
            var i = 0;
            var result = new List<Message>();

            do
            {
                var currentItems = _messages.GetHistory(id, isChat, out totalCount, i * count, count, inReverse, startMessageId);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех диалогов текущего пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, последнее сообщение в переписке с которым необходимо вернуть.</param>
        /// <param name="totalCount">Общее количество диалогов с учетом фильтра. Если получены только диалоги, в которых есть непрочитанные сообщения, то вернет то же что и unreadCount</param>
        /// <param name="unreadCount">Количество диалогов с непрочитанными сообщениями</param>
        /// <param name="unread">Значение true означает, что нужно вернуть только диалоги в которых есть непрочитанные входящие сообщения. По умолчанию false.</param>
        /// <param name="chatId">Идентификатор беседы, последнее сообщение в которой необходимо вернуть.</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение. 
        /// Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются).</param>
        /// <returns>Список диалогов текущего пользователя.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/messages.getDialogs"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.28")]
        public ReadOnlyCollection<Message> GetAllDialogs(out int totalCount, out int unreadCount, bool unread = false, long? userId = null, long? chatId = null, int? previewLength = null)
        {
            const int count = 200;
            var i = 0;
            var result = new List<Message>();

            do
            {
                var currentItems = _messages.GetDialogs(count, i * count, out totalCount, out unreadCount, unread, userId, chatId, previewLength);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех найденных личных сообщений текущего пользователя по введенной строке поиска. 
        /// </summary>
        /// <param name="query">Подстрока, по которой будет производиться поиск.</param>
        /// <param name="totalCount">Общее количество найденных сообщений.</param>
        /// <returns>Список личных сообщений пользователя, удовлетворяющих условиям запроса.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/messages.search"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Message> SearchAll([NotNull] string query, out int totalCount)
        {
            const int count = 100;
            var i = 0;
            var result = new List<Message>();

            do
            {
                var currentItems = _messages.Search(query, out totalCount, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

    }
}
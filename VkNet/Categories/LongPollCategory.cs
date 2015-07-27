using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.LongPollEvents;
using VkNet.Utils;

namespace VkNet.Categories
{
    public class LongPollCategory: BaseCategory
    {
        public const int DefaultWaitTime = 25;
        public LongPollCategory(VkApi vk): base(vk)
        {
        }

        /// <summary>
        /// Возвращает данные, необходимые для подключения к Long Poll серверу. 
        /// Long Poll подключение позволит Вам моментально узнавать о приходе новых сообщений и других событий. 
        /// </summary>
        /// <returns>
        /// Возвращает объект, с помощью которого можно подключиться к серверу быстрых сообщений для мгновенного 
        /// получения приходящих сообщений и других событий.  
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/messages.getLongPollServer"/>.
        /// </remarks>
        [Pure]
        public LongPollServerResponse GetLongPollServer(bool? needPts = null)
        {
            var parameters = new VkParameters
                             {
                                 { "need_pts", needPts }
                             };
            return _vk.Call("messages.getLongPollServer", parameters);
        }

        /// <summary>
        /// Возвращает обновления в личных сообщениях пользователя. 
        /// Для ускорения работы с личными сообщениями может быть полезно кешировать уже загруженные ранее сообщения на 
        /// мобильном устройстве / ПК пользователя, чтобы не получать их повторно при каждом обращении. 
        /// Этот метод помогает осуществить синхронизацию локальной копии списка сообщений с актуальной версией. 
        /// </summary>
        /// <param name="ts">Последнее значение параметра ts, полученное от Long Poll сервера или с помощью метода</param>
        /// <param name="pts">Последнее значение параметра new_pts, полученное от Long Poll сервера, используется для получения действий, которые хранятся всегда. </param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение.</param>
        /// <param name="onlines">True если должна быть возвращена история только от тех пользователей, которые сейчас online</param>
        /// <param name="fields">Поля профилей, которые необходимо возвратить.
        /// По умолчанию photo,photo_medium_rec,sex,online,screen_name</param>
        /// <param name="eventsLimit">Если количество событий в истории превысит это значение, будет возвращена ошибка.
        /// По умолчанию 1000. Минимальное значение 1000.</param>
        /// <param name="messagesLimit">Количество сообщений, которое нужно вернуть. 
        /// По умолчанию 200, минимальное значение 200</param>
        /// <param name="maxMessageId">Максимальный идентификатор сообщения среди уже имеющихся в локальной копии. 
        /// Необходимо учитывать как сообщения, полученные через методы API (например messages.getDialogs, messages.getHistory), так и данные, полученные из Long Poll сервера (события с кодом 4).</param>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/messages.getLongPollHistory"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.29")]
        public async Task<LongPollHistoryResponse> GetLongPollHistoryAsync(long ts, long pts,
            int? previewLength = null,
            bool? onlines = null,
            ProfileFields fields = null,
            int? eventsLimit = null,
            int? messagesLimit = null,
            long? maxMessageId = null)
        {
            var parameters = new VkParameters
                             {
                                 { "ts", ts },
                                 { "pts", pts },
                                 { "preview_length", previewLength },
                                 { "onlines", onlines },
                                 { "fields", fields },
                                 { "events_limit", eventsLimit },
                                 { "msgs_limit", messagesLimit },
                                 { "max_msg_id", maxMessageId }
                             };

            VkResponse response = await _vk.CallAsync("messages.getLongPollHistory", parameters);

            bool hasMoreMessages = response.ContainsKey("more") && response["more"] == 1;
            var messages = response["items"].ToReadOnlyCollectionOf<Message>(r => r);

            var result = new LongPollHistoryResponse()
            {
                HasMoreMessages = hasMoreMessages,
                Messages = messages,
            };
            return result;
        }

        /// <summary>
        /// Возвращает обновления в личных сообщениях пользователя. 
        /// Для ускорения работы с личными сообщениями может быть полезно кешировать уже загруженные ранее сообщения на 
        /// мобильном устройстве / ПК пользователя, чтобы не получать их повторно при каждом обращении. 
        /// Этот метод помогает осуществить синхронизацию локальной копии списка сообщений с актуальной версией. 
        /// </summary>
        /// <param name="ts">Последнее значение параметра ts, полученное от Long Poll сервера или с помощью метода</param>
        /// <param name="pts">Последнее значение параметра new_pts, полученное от Long Poll сервера, используется для получения действий, которые хранятся всегда. </param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение.</param>
        /// <param name="onlines">True если должна быть возвращена история только от тех пользователей, которые сейчас online</param>
        /// <param name="fields">Поля профилей, которые необходимо возвратить.
        /// По умолчанию photo,photo_medium_rec,sex,online,screen_name</param>
        /// <param name="eventsLimit">Если количество событий в истории превысит это значение, будет возвращена ошибка.
        /// По умолчанию 1000. Минимальное значение 1000.</param>
        /// <param name="messagesLimit">Количество сообщений, которое нужно вернуть. 
        /// По умолчанию 200, минимальное значение 200</param>
        /// <param name="maxMessageId">Максимальный идентификатор сообщения среди уже имеющихся в локальной копии. 
        /// Необходимо учитывать как сообщения, полученные через методы API (например messages.getDialogs, messages.getHistory), так и данные, полученные из Long Poll сервера (события с кодом 4).</param>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/messages.getLongPollHistory"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.29")]
        public LongPollHistoryResponse GetLongPollHistory(long ts, 
            long? pts = null,
            int? previewLength = null,
            bool? onlines = null,
            ProfileFields fields = null,
            int? eventsLimit = null,
            int? messagesLimit = null,
            long? maxMessageId = null)
        {
            var parameters = new VkParameters
                             {
                                 { "ts", ts },
                                 { "pts", pts },
                                 { "preview_length", previewLength },
                                 { "onlines", onlines },
                                 { "fields", fields },
                                 { "events_limit", eventsLimit },
                                 { "msgs_limit", messagesLimit },
                                 { "max_msg_id", maxMessageId }
                             };

            VkResponse response = _vk.Call("messages.getLongPollHistory", parameters);

            bool hasMoreMessages = response.ContainsKey("more") && response["more"] == 1;
            var messages = response["messages"].ToReadOnlyCollectionOf<Message>(r => r);
            var newPts = (long?)response["new_pts"];

            var result = new LongPollHistoryResponse()
            {
                HasMoreMessages = hasMoreMessages,
                Messages = messages,
                Pts = newPts
            };
            return result;
        }

        public LongPollResponse GetLongPollEvents(string server, string key, long ts, LongPollMods mods = LongPollMods.IncludeAttachemnts)
        {
            var parameters = new VkParameters
                             {
                                 { "act", "a_check" },
                                 { "key", key },
                                 { "ts", ts },
                                 { "wait", DefaultWaitTime }
                             };

            VkResponse response = _vk.CallLongPoll(server, parameters);

            var result = new LongPollResponse()
            {
                Ts = response["ts"]
            };
            var events = response["updates"]
                .ToReadOnlyCollectionOf<VkResponseArray>(u => u)
                .Select(LongPollEventBase.FromArray)
                .Where(e => e != null)
                .ToReadOnlyCollection();
            result.Events = events;
            return result;
        }
    }
}

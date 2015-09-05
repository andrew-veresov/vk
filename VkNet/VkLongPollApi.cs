using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.LongPollEvents;
using VkNet.Utils;

namespace VkNet
{
    public class VkLongPollApi
    {
        private readonly VkApi _api;
        private Lazy<IObservable<Message>> _messages;

        /// <summary>
        /// Last processed PTS value
        /// </summary>
        public long? LastPTS { get; protected set; }

        /// <summary>
        /// Highest processed Message ID
        /// </summary>
        public long? MaxMessageId { get; private set; }

        /// <summary>
        /// Get Messages Observable
        /// </summary>
        public IObservable<Message> Messages => _messages.Value;

        /// <summary>
        /// Возвращает объек позволяющий получать уведомления от Long Poll сервера
        /// </summary>
        /// <param name="api">VkApi для доступа к серверу Вконтакте</param>
        public VkLongPollApi(VkApi api)
        {
            if (api.UserId == null) throw new System.Exception("Переданный VkApi не инициализирован. Пожалуйста авторизуйтесь.");
            if (!api.HasPermission(Permissions.Messages)) throw new System.Exception("Приложению не выданы права: messages.");

            _api = api;

            _messages = new Lazy<IObservable<Message>>(ConstructMessagesObservable, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Возвращает объек позволяющий получать уведомления от Long Poll сервера
        /// </summary>
        /// <param name="api"></param>
        /// <param name="lastPts">Последнее обработанное PTS</param>
        public VkLongPollApi(VkApi api, long lastPts): this(api)
        {
            LastPTS = lastPts;
        }

        private IObservable<Message> ConstructMessagesObservable()
        {
            var messages = Observable.Create<Message>(obs =>
            {
                return Task.Run(() =>
                {
                    var serverInfo = _api.LongPoll.GetLongPollServer(true);

                    LongPollHistoryResponse history;
                    do
                    {
                        history = _api.LongPoll.GetLongPollHistory(serverInfo.Ts, maxMessageId: MaxMessageId,
                            pts: LastPTS ?? serverInfo.Pts);
                        if (history == null) continue;

                        LastPTS = history.Pts;

                        foreach (var message in history.Messages)
                        {
                            if ((MaxMessageId ?? 0) < message.Id)
                            {
                                MaxMessageId = message.Id;
                            }
                            obs.OnNext(message);
                        }
                    } while (history?.HasMoreMessages ?? false);

                    var lastTS = serverInfo.Ts;
                    do
                    {
                        var updates = _api.LongPoll.GetLongPollEvents(serverInfo.Server, serverInfo.Key, lastTS);
                        lastTS = updates.Ts;
                        foreach (var messageEvent in updates.Events.OfType<NewMessageEvent>())
                        {
                            obs.OnNext(messageEvent.Message);
                        }
                    } while (true);
                });
            });
            return messages;
        }
    }
}

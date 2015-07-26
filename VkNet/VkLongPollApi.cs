using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet
{
    public class VkLongPollApi
    {
        private readonly VkApi _api;
        private LongPollServerResponse _serverInfo;

        public long LastTS { get; protected set; }
        public long? MaxMessageId { get; private set; }

        public IObservable<Message> GetMessagesObservable()
        {
            return ConstructHistoryObservable();
            if (LastTS != 0)
            {
                return ConstructHistoryObservable();
            }
            else
            {
                return ConstructHistoryObservable()
                    .Concat(ConstructLongPollObservable());
            }
        }

        public IObservable<Message> ConstructHistoryObservable()
        {
            var observable = Observable.Create<Message>(obs =>
                {
                    return Task.Run(() =>
                    {
                        LongPollHistoryResponse history = null;
                        do
                        {
                            history = _api.LongPoll.GetLongPollHistory(LastTS, maxMessageId: MaxMessageId, pts: history?.Pts);
                            
                            foreach (var message in history.Messages)
                            {
                                if ((MaxMessageId ?? 0) < message.Id)
                                {
                                    MaxMessageId = message.Id;
                                }
                                obs.OnNext(message);
                            }
                        } while (history.Messages.Count > 0);

                        obs.OnCompleted();
                    });
                }
            );

            return observable;
        }

        public IObservable<Message> ConstructLongPollObservable()
        {
            var observable = Observable.Create<Message>(observer =>
            {
                return Task.Run(() =>
                {
                    var _serverInfo = _api.LongPoll.GetLongPollServer();
                    Console.WriteLine("TimeStamp: " + _serverInfo.Ts);
                    observer.OnCompleted();
                });
            });

            return observable;
        }
        //public IObservable<Message> Messages = Observable.Create<Message>(observer =>
        //{
        //    if (LastTS != 0)
        //    {

        //    }
        //})

        /// <summary>
        /// Возвращает объек позволяющий получать уведомления от Long Poll сервера
        /// </summary>
        /// <param name="api">VkApi для доступа к серверу Вконтакте</param>
        public VkLongPollApi(VkApi api)
        {
            if (api.UserId == null) throw new System.Exception("Переданный VkApi не инициализирован. Пожалуйста авторизуйтесь.");
            if (!api.HasPermission(Permissions.Messages)) throw new System.Exception("Приложению не выданы права: messages.");

            _api = api;
        }

        public VkLongPollApi(VkApi api, long lastTs): this(api)
        {
            LastTS = lastTs;
        }
        
    }
}

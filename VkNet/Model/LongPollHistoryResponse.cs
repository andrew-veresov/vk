using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace VkNet.Model
{
    /// <summary>
    /// Объект, содержащий список сообщений и список событий, полученных в результате вызова метода  <see href="http://vk.com/dev/messages.getLongPollHistory"/>.
    /// </summary>
    public class LongPollHistoryResponse
    {
        public ReadOnlyCollection<object> History {get { throw new NotImplementedException(); } }
        public ReadOnlyCollection<Message> Messages { get; internal set; }
        public bool HasMoreMessages { get; internal set; }
        public long Pts { get; internal set; }
    }
}

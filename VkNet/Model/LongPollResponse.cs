using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkNet.Model.LongPollEvents;

namespace VkNet.Model
{
    /// <summary>
    /// Объект, содержащий набор событий возвращаемых Long Poll сервером
    /// </summary>
    public class LongPollResponse
    {
        /// <summary>
        /// Отметка времени.
        /// </summary>
        public long Ts { get; internal set; }

        /// <summary>
        /// События
        /// </summary>
        public IReadOnlyCollection<LongPollEventBase> Events { get; internal set; }
    }
}

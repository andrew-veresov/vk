using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public long Ts { get; set; }
    }
}

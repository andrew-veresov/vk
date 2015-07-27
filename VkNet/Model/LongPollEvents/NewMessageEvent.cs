using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Enums;
using VkNet.Utils;

namespace VkNet.Model.LongPollEvents
{
    public class NewMessageEvent: LongPollEventBase
    {
        public Message Message { get; set; }

        internal NewMessageEvent(): base(LongPollEventTypes.NewMessage)
        {
             
        }

        internal new static LongPollEventBase FromArray(VkResponseArray eventArray)
        {
            var result = new NewMessageEvent()
            {
                Message = new Message()
                {
                    Id = eventArray[1],
                    UserId = eventArray[3],
                    Date = eventArray[4],
                    Title = eventArray[5],
                    Body = eventArray[6],
                    Attachments = eventArray.Count >= 8 ? eventArray[7] : null
                }
            };
            return result;
        }
    }
}

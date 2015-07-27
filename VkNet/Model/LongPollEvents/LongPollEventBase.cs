using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Enums;
using VkNet.Utils;

namespace VkNet.Model.LongPollEvents
{
    public abstract class LongPollEventBase
    {
        protected LongPollEventTypes EventType { get; private set; }

        internal LongPollEventBase(LongPollEventTypes eventType)
        {
            EventType = eventType;
        }

        internal static LongPollEventBase FromArray(VkResponseArray eventArray)
        {
            switch ((LongPollEventTypes)eventArray[0])
            {
                case LongPollEventTypes.MessageDeleted:
                    break;
                case LongPollEventTypes.MessageFlagsReplaced:
                    break;
                case LongPollEventTypes.MessageFlagsSet:
                    break;
                case LongPollEventTypes.MessageFlagsClear:
                    break;
                case LongPollEventTypes.NewMessage:
                    return NewMessageEvent.FromArray(eventArray);
                case LongPollEventTypes.IncomingMessagesRead:
                    break;
                case LongPollEventTypes.OutgoingMessagesRead:
                    break;
                case LongPollEventTypes.FriendOnline:
                    break;
                case LongPollEventTypes.FriendOffline:
                    break;
                case LongPollEventTypes.ChatParameterChanged:
                    break;
                case LongPollEventTypes.UserStartTyping:
                    break;
                case LongPollEventTypes.UserStartTypingInChat:
                    break;
                case LongPollEventTypes.UserCall:
                    break;
                case LongPollEventTypes.UnreadMessagesChanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }
    }
}

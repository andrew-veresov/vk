using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkNet.Enums
{
    public enum LongPollEventTypes
    {
        /// <summary>
        /// 0,$message_id,0 — удаление сообщения с указанным local_id
        /// </summary>
        MessageDeleted = 0,

        /// <summary>
        /// 1,$message_id,$flags — замена флагов сообщения (FLAGS:=$flags)
        /// </summary>
        MessageFlagsReplaced = 1,

        /// <summary>
        /// 2,$message_id,$mask[,$user_id] — установка флагов сообщения (FLAGS|=$mask)
        /// </summary>
        MessageFlagsSet = 2,

        /// <summary>
        /// 3,$message_id,$mask[,$user_id] — сброс флагов сообщения (FLAGS&=~$mask)
        /// </summary>
        MessageFlagsClear = 3,

        /// <summary>
        /// 4,$message_id,$flags,$from_id,$timestamp,$subject,$text,$attachments — добавление нового сообщения
        /// </summary>
        NewMessage = 4,

        /// <summary>
        /// 6,$peer_id,$local_id — прочтение всех входящих сообщений с $peer_id вплоть до $local_id включительно
        /// </summary>
        IncomingMessagesRead = 6,

        /// <summary>
        /// 7,$peer_id,$local_id — прочтение всех исходящих сообщений с $peer_id вплоть до $local_id включительно
        /// </summary>
        OutgoingMessagesRead = 7,

        /// <summary>
        /// 8,-$user_id,$extra — друг $user_id стал онлайн, $extra не равен 0, если в mode был передан флаг 64, в младшем байте (остаток от деления на 256) числа $extra лежит идентификатор платформы (таблица ниже)
        /// </summary>
        FriendOnline = 8,

        /// <summary>
        /// 9,-$user_id,$flags — друг $user_id стал оффлайн ($flags равен 0, если пользователь покинул сайт (например, нажал выход) и 1, если оффлайн по таймауту (например, статус away))
        /// </summary>
        FriendOffline = 9,

        /// <summary>
        /// 51,$chat_id,$self — один из параметров (состав, тема) беседы $chat_id были изменены. $self - были ли изменения вызваны самим пользователем
        /// </summary>
        ChatParameterChanged = 51,

        /// <summary>
        /// 61,$user_id,$flags — пользователь $user_id начал набирать текст в диалоге. событие должно приходить раз в ~5 секунд при постоянном наборе текста. $flags = 1
        /// </summary>
        UserStartTyping = 61,

        /// <summary>
        /// 62,$user_id,$chat_id — пользователь $user_id начал набирать текст в беседе $chat_id
        /// </summary>
        UserStartTypingInChat = 62,

        /// <summary>
        /// 70,$user_id,$call_id — пользователь $user_id совершил звонок имеющий идентификатор $call_id
        /// </summary>
        UserCall = 70,

        /// <summary>
        /// 80,$count,0 — новый счетчик непрочитанных в левом меню стал равен $count
        /// </summary>
        UnreadMessagesChanged = 80
    }
}

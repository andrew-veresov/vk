using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace VkNet.Enums
{
    [Flags]
    public enum Permissions: ulong
    {
        None = 0,
        [Description("Пользователь разрешил отправлять ему уведомления (для flash/ifrаme-приложений)")]
        Notify = 1,
        [Description("Доступ к друзьям")]
        Friends = 2,
        [Description("Доступ к фотографиям")]
        Photos = 4,
        [Description("Доступ к аудиозаписям")]
        Audio = 8,
        [Description("Доступ к видеозаписям")]
        Video = 16,
        [Description("Доступ к предложениям (устаревшие методы)")]
        Offers = 32,
        [Description("Доступ к вопросам (устаревшие методы)")]
        Questions = 64,
        [Description("Доступ к wiki-страницам")]
        Pages = 128,
        [Description("Добавление ссылки на приложение в меню слева")]
        AddAppLink = 256,
        [Description("Доступ к статусу пользователя")]
        Status = 1024,
        [Description("Доступ к заметкам пользователя")]
        Notes = 2048,
        [Description("(для Standalone-приложений) Доступ к расширенным методам работы с сообщениями")]
        Messages = 4096,
        [Description("Доступ к обычным и расширенным методам работы со стеной. Внимание: данное право доступа недоступно для сайтов (игнорируется при попытке авторизации)")]
        Wall = 8192,
        [Description("Доступ к расширенным методам работы с рекламным API")]
        Ads = 32768,
        [Description("Доступ к API в любое время со стороннего сервера (при использовании этой опции параметр expires_in, возвращаемый вместе с access_token, содержит 0 — токен бессрочный)")]
        Offline = 65536,
        [Description("Доступ к документам")]
        Docs = 131072,
        [Description("Доступ к группам пользователя")]
        Groups = 262144,
        [Description("Доступ к оповещениям об ответах пользователю")]
        Notifications = 524288,
        [Description("Доступ к статистике групп и приложений пользователя, администратором которых он является")]
        Stats = 1048576,
        [Description("Доступ к email пользователя")]
        Email = 4194304,
        //Nohttps Возможность осуществлять запросы к API без HTTPS.
    }
}

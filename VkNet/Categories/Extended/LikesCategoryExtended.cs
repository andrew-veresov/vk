using System.Collections.Generic;
using System.Collections.ObjectModel;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы с отметками "Мне нравится".
    /// </summary>
    public class LikesCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly LikesCategory _likes;

        internal LikesCategoryExtended(LikesCategory likesCategory, VkApi vk)
        {
            _likes = likesCategory;
            _vk = vk;
        }

        /// <summary>
        /// Получает список всех идентификаторов пользователей, которые добавили заданный объект в свой список Мне нравится. 
        /// </summary>
        /// <param name="type">Тип объекта <see cref="LikeObjectType"/></param>
        /// <param name="ownerId">Идентификатор владельца Like-объекта: id пользователя, id сообщества (со знаком "минус") или id приложения. Если параметр type равен sitepage, то в качестве owner_id необходимо передавать id приложения. Если параметр не задан, то считается, что он равен либо идентификатору текущего пользователя, либо идентификатору текущего приложения (если type равен sitepage). целое число</param>
        /// <param name="itemId">Идентификатор Like-объекта. Если type равен sitepage, то параметр item_id может содержать значение параметра page_id, используемый при инициализации  виджета "Мне нравится".</param>
        /// <param name="pageUrl">Url страницы, на которой установлен виджет "Мне нравится". Используется вместо параметра item_id. строка</param>
        /// <param name="filter">Указывает, следует ли вернуть всех пользователей, добавивших объект в список &quot;Мне нравится&quot; или только тех, которые рассказали о нем друзьям. Параметр может принимать следующие значения: 
        /// 
        /// likes — возвращать информацию обо всех пользователях; 
        /// copies — возвращать информацию только о пользователях, рассказавших об объекте друзьям.
        /// По умолчанию возвращается информация обо всех пользователях. 
        /// строка</param>
        /// <param name="friendsOnly">Указывает, необходимо ли возвращать только пользователей, которые являются друзьями текущего пользователя. Параметр может принимать следующие значения: 
        /// 
        /// 0 — возвращать всех пользователей в порядке убывания времени добавления объекта; 
        /// 1 — возвращать только друзей текущего пользователя в порядке убывания времени добавления объекта;
        /// Если метод был вызван без авторизации или параметр не был задан, то считается, что он равен 0. 
        /// флаг, может принимать значения 1 или 0</param>
        /// <param name="extended">1 — возвращать расширенную информацию о пользователях и сообществах из списка поставивших отметку "Мне нравится" или сделавших репост. По умолчанию — 0. флаг, может принимать значения 1 или 0</param>
        /// <returns>После успешного выполнения возвращает объект со следующими полями: 
        /// 
        /// count — общее количество пользователей, которые добавили заданный объект в свой список Мне нравится. 
        /// items — список идентификаторов пользователей с учетом параметров offset и count, которые добавили заданный объект в свой список Мне нравится. 
        /// 
        /// Если параметр type равен sitepage, то будет возвращён список пользователей, воспользовавшихся виджетом "Мне нравится" на внешнем сайте. Адрес страницы задаётся при помощи параметра page_url или item_id. 
        /// Если extended=1, дополнительно возвращается массив items, содержащий расширенную информацию о пользователях или сообществах. </returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/likes.getList"/>.
        /// </remarks>
        [ApiVersion("5.29")]
        public ReadOnlyCollection<User> GetAllList(LikeObjectType type, long? ownerId = null, long? itemId = null, string pageUrl = null, string filter = null, bool? friendsOnly = null, bool? extended = null)
        {
            var count = friendsOnly.HasValue ? 100 : 1000;
            var i = 0;
            var result = new List<User>();

            do
            {
                var currentItems = _likes.GetList(type, ownerId, itemId, pageUrl, filter, friendsOnly, extended, i * count, count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
    }
}
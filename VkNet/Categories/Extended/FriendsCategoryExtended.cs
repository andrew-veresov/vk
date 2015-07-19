using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы с друзьями.
    /// </summary>
    public class FriendsCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly FriendsCategory _friends;

        internal FriendsCategoryExtended(FriendsCategory friendsCategory, VkApi vk)
        {
            _friends = friendsCategory;
            _vk = vk;
        }

        /// <summary>
        /// Возвращает список всех идентификаторов друзей пользователя или расширенную информацию о друзьях пользователя (при использовании параметра fields).
        /// </summary>
        /// <param name="uid">Идентификатор пользователя, для которого необходимо получить список друзей.</param>
        /// <param name="fields">Поля анкеты (профиля), которые необходимо получить.</param>
        /// <param name="order">Порядок, в котором нужно вернуть список друзей.</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя.</param>
        /// <param name="listId">Идентификатор списка друзей, полученный методом <see cref="FriendsCategory.GetLists"/>, друзей из которого необходимо получить. Данный параметр учитывается, только когда параметр uid равен идентификатору текущего пользователя.</param>
        /// <returns>Список друзей пользователя с заполненными полями (указанными в параметре <paramref name="fields"/>).
        /// Если значение поля <paramref name="fields"/> не указано, то у возвращаемых друзей заполняется только поле Id.
        /// </returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/friends.get"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.24")]
        public ReadOnlyCollection<User> GetAll(long uid, ProfileFields fields = null, FriendsOrder order = null, NameCase nameCase = null, int? listId = null)
        {
            const int count = 50;
            var i = 0;
            var result = new List<User>();

            do
            {
                var currentItems = _friends.Get(uid, fields, count, i * count, order, nameCase, listId);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает информацию о всех полученных или отправленных заявках на добавление в друзья для текущего пользователя
        /// </summary>
        /// <param name="extended">Определяет, требуется ли возвращать в ответе сообщения от пользователей, подавших заявку на добавление 
        /// в друзья. И отправителя рекомендации при suggested=true.</param>
        /// <param name="needMutual">Определяет, требуется ли возвращать в ответе список общих друзей, если они есть. Обратите внимание, 
        /// что при использовании need_mutual будет возвращено не более 20 заявок.</param>
        /// <param name="out">false — возвращать полученные заявки в друзья (по умолчанию), true — возвращать отправленные пользователем 
        /// заявки.</param>
        /// <param name="sort">false — сортировать по дате добавления, true — сортировать по количеству общих друзей. (Если out = true, 
        /// данный параметр не учитывается).</param>
        /// <param name="suggested">true — возвращать рекомендованных другими пользователями друзей, false — возвращать заявки в друзья 
        /// (по умолчанию).</param>
        /// <returns>
        /// - Если не установлен параметр need_mutual, то в случае успеха возвращает отсортированный в антихронологическом порядке по 
        /// времени подачи заявки список идентификаторов (id) пользователей (кому или от кого пришла заявка).
        /// - Если установлен параметр need_mutual, то в случае успеха возвращает отсортированный в антихронологическом порядке по 
        /// времени подачи заявки массив объектов, содержащих информацию о заявках на добавление в друзья. Каждый из объектов содержит 
        /// поле uid, являющийся идентификатором пользователя. При наличии общих друзей, в объекте будет содержаться поле mutual, в 
        /// котором будет находиться список идентификаторов общих друзей.
        /// </returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/friends.getRequests"/>.
        /// </remarks>
        // todo add more tests on out, suggested and mutual params
        [Pure]
        public ReadOnlyCollection<long> GetAllRequests(bool extended = false, bool needMutual = false, bool @out = false, bool sort = false, bool suggested = false)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<long>();

            do
            {
                var currentItems = _friends.GetRequests(count, i * count, extended, needMutual, @out, sort, suggested);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
        
    }
}
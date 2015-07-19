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
    /// Расширения категории работы с сообществами (группами).
    /// </summary>
    public class GroupsCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly GroupsCategory _groups;

        internal GroupsCategoryExtended(GroupsCategory groupsCategory, VkApi vk)
        {
            _groups = groupsCategory;
            _vk = vk;
        }

        /// <summary>
        /// Возвращает список всех групп указанного пользователя.
        /// </summary>
        /// <param name="uid">Id пользователя</param>
        /// <param name="extended">Возвращать полную информацию?</param>
        /// <param name="filters">Список фильтров сообществ</param>
        /// <param name="fields">Список полей информации о группах</param>
        /// <returns>Список групп</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/groups.get"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.28")]
        public ReadOnlyCollection<Group> GetAll(long uid, bool extended = false, GroupsFilters filters = null, GroupsFields fields = null)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<Group>();

            do
            {
                var currentItems = _groups.Get(uid, extended, filters, fields, i * count, count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех участников группы.
        /// </summary>
        /// <param name="gid">Id группы</param>
        /// <param name="totalCount">Общее количество участников</param>
        /// <param name="sort">Сортировка Id пользователей</param>
        /// <returns>Id пользователей состоящих в группе</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/groups.getMembers"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<long> GetAllMembers(long gid, out int totalCount, GroupsSort sort = null)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<long>();

            do
            {
                var currentItems = _groups.GetMembers(gid, out totalCount, count, i * count, sort);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Осуществляет поиск всех групп по заданной подстроке.
        /// </summary>
        /// <param name="query">Поисковый запрос</param>
        /// <param name="totalCount">Общее количество групп удовлетворяющих запросу</param>
        /// <returns>Список объектов групп</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/groups.search"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Group> SearchAll([NotNull] string query, out int totalCount)
        {
            const int count = 1000;
            var result = new List<Group>();

            var currentItems = _groups.Search(query, out totalCount, 0, count);
            if (currentItems != null) result.AddRange(currentItems);

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Данный метод возвращает список всех приглашений в сообщества и встречи.
        /// </summary>
        /// <returns>После успешного выполнения возвращает список объектов сообществ с дополнительным полем InvitedBy, содержащим идентификатор пользователя, который отправил приглашение.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/groups.getInvites"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Group> GetAllInvites()
        {
            const int count = 20;
            var i = 0;
            var result = new List<Group>();

            do
            {
                var currentItems = _groups.GetInvites(count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех забаненных пользователей в сообществе
        /// </summary>
        /// <param name="groupId">идентификатор сообщества</param>
        /// <returns>После успешного выполнения возвращает список объектов пользователей с дополнительным полем <see cref="BanInfo"/></returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/groups.getBanned"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<User> GetAllBanned(long groupId)
        {
            const int count = 200;
            var i = 0;
            var result = new List<User>();

            do
            {
                var currentItems = _groups.GetBanned(groupId, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
    }
}
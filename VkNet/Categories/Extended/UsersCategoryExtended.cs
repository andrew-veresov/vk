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
    /// Расширения категории работы с информацией о пользователях.
    /// </summary>
    public class UsersCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly UsersCategory _users;

        internal UsersCategoryExtended(UsersCategory usersCategory, VkApi vk)
        {
            _users = usersCategory;
            _vk = vk;
        }


        /// <summary>
        /// Возвращает список всех (максимум - 1000 первых) пользователей в соответствии с заданным критерием поиска.
        /// </summary>
        /// <param name="query">Строка поискового запроса. Например, Вася Бабич.</param>
        /// <param name="itemsCount">Общее количество пользователей, удовлетворяющих условиям запроса.</param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть.</param>
        /// <returns>
        /// После успешного выполнения возвращает список объектов пользователей, найденных в соответствии с заданными критериями. 
        /// </returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/users.search"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<User> SearchAll([NotNull] string query, out int itemsCount, ProfileFields fields = null)
        {
            return _users.Search(query, out itemsCount, fields, 1000, 0);
        }


        // todo add tests for subscriptions for users
        /// <summary>
        /// Возвращает список идентификаторов групп, которые входят в список подписок пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, подписки которого необходимо получить</param>
        /// <returns>Пока возвращается только список групп.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/users.getSubscriptions"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Group> GetAllSubscriptions(long? userId = null)
        {
            const int count = 200;
            var i = 0;
            var result = new List<Group>();

            do
            {
                var currentItems = _users.GetSubscriptions(userId, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список идентификаторов пользователей, которые являются подписчиками пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя</param>
        /// <returns>Список подписчиков</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/users.getFollowers"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<User> GetAllFollowers(long? userId = null, ProfileFields fields = null, NameCase nameCase = null)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<User>();

            do
            {
                var currentItems = _users.GetFollowers(userId, count, i * count, fields, nameCase);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
        
    }
}
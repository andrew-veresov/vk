using System.Collections.Generic;
using JetBrains.Annotations;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы с аккаунтом пользователя.
    /// </summary>
    public class AccountCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly AccountCategory _account;

        internal AccountCategoryExtended(AccountCategory accountCategory, VkApi vk)
        {
            _account = accountCategory;
            _vk = vk;
        }


        /// <summary>
        /// Возвращает список всех пользователей, находящихся в черном списке. 
        /// </summary>
        /// <returns>Возвращает набор объектов пользователей, находящихся в черном списке. </returns>
        [Pure]
        [ApiVersion("5.21")]
        public IEnumerable<User> GetAllBanned()
        {
            const int count = 200;
            var i = 0;
            var result = new List<User>();

            do
            {
                int total;
                var currentItems = _account.GetBanned(out total, i * count, count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
    }
}
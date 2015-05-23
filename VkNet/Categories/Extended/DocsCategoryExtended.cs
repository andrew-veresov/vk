using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы с документами.
    /// </summary>
    public class DocsCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly DocsCategory _docs;

        internal DocsCategoryExtended(DocsCategory docsCategory, VkApi vk)
        {
            _docs = docsCategory;
            _vk = vk;
        }

        /// <summary>
        /// Возвращает расширенную информацию о всех документах пользователя или сообщества.
        /// </summary>
        /// <param name="owner_id">Идентификатор пользователя или сообщества, которому принадлежат документы. Целое число, по умолчанию идентификатор текущего пользователя.</param>
        /// <returns>После успешного выполнения возвращает список объектов документов.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/docs.get"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.28")]
        public ReadOnlyCollection<Document> GetAll(long? owner_id = null)
        {
            const int count = 50;
            var i = 0;
            var result = new List<Document>();

            do
            {
                var currentItems = _docs.Get(count, i * count, owner_id);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
    }
}
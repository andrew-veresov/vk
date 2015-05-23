using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using VkNet.Enums;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы со стеной пользователя.
    /// </summary>
    public class WallCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly WallCategory _wall;

        internal WallCategoryExtended(WallCategory wallCategory, VkApi vk)
        {
            _wall = wallCategory;
            _vk = vk;
        }

        /// <summary>
        /// Возвращает список всех записей со стены пользователя или сообщества. 
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя. Чтобы получить записи со стены группы (публичной страницы, встречи), укажите её идентификатор 
        /// со знаком "минус": например, owner_id=-1 соответствует группе с идентификатором 1.</param>
        /// <param name="filter">Типы сообщений, которые необходимо получить (по умолчанию возвращаются все сообщения).</param>
        /// <returns>В случае успеха возвращается запрошенный список записей со стены.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/wall.get"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Post> GetAll(long ownerId, WallFilter filter = WallFilter.All)
        {
            const int count = 100;
            var i = 0;
            var allPosts = new List<Post>();

            do
            {
                int totalCount;
                var currentPosts = _wall.Get(ownerId, out totalCount, count, i * count, filter);
                if (currentPosts != null) allPosts.AddRange(currentPosts);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return allPosts.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех комментариев к записи на стене пользователя. 
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя, на чьей стене находится запись, к которой необходимо получить комментарии.</param>
        /// <param name="postId">Идентификатор записи на стене пользователя.</param>
        /// <param name="sort">Порядок сортировки комментариев (по умолчанию хронологический).</param>
        /// <param name="needLikes">Признак нужно ли возвращать поле Likes в комментариях.</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать комментарии. Если указано 0, то комментарии не обрезаются. 
        /// Обратите внимание, что комментарии обрезаются по словам.</param>
        /// <returns>
        /// Список комментариев к записи на стене пользователя.
        /// </returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/wall.getComments"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Comment> GetAllComments(
            long ownerId,
            long postId,
            CommentsSort sort = null,
            bool needLikes = false,
            int previewLength = 0)
        {
            const int count = 100;
            var i = 0;
            var allComments = new List<Comment>();

            do
            {
                int totalCount;
                var currentComments = _wall.GetComments(ownerId, postId, out totalCount, sort, needLikes, count, i * count, previewLength);
                if (currentComments != null) allComments.AddRange(currentComments);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return allComments.ToReadOnlyCollection();
        }
        
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы с видеофайлами.
    /// </summary>
    public class VideoCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly VideoCategory _video;

        internal VideoCategoryExtended(VideoCategory videoCategory, VkApi vk)
        {
            _video = videoCategory;
            _vk = vk;
        }

        /// <summary>
        /// Возвращает информацию о всех видеозаписях.
        /// </summary>
        /// <param name="ownerId">
        /// Идентификатор пользователя или сообщества, которому принадлежат видеозаписи.
        /// Обратите внимание, идентификатор сообщества в параметре <paramref name="ownerId"/> необходимо указывать со знаком "-" — например, 
        /// <paramref name="ownerId"/>=-1 соответствует идентификатору сообщества ВКонтакте API (club1).
        /// </param>
        /// <param name="albumId">Идентификатор альбома, видеозаписи из которого нужно вернуть.</param>
        /// <param name="width">Требуемая ширина изображений видеозаписей в пикселах.</param>
        /// <param name="extended">Определяет, возвращать ли информацию о настройках приватности видео для текущего пользователя.</param>
        /// <returns>После успешного выполнения возвращает список объектов видеозаписей с дополнительным полем comments, содержащим число комментариев у 
        /// видеозаписи. Если был задан параметр <paramref name="extended"/>, то для каждой видеозаписи возвращаются дополнительные поля: 
        /// <see cref="Video.CanComment"/>, <see cref="Video.CanRepost"/>, <see cref="Video.Likes"/></returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Video"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/video.get"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Video> GetAll(long? ownerId = null, long? albumId = null, VideoWidth width = VideoWidth.Medium160, bool extended = false)
        {
            const int count = 200;
            var i = 0;
            var result = new List<Video>();

            do
            {
                var currentItems = _video.Get(ownerId, albumId, width, count, i * count, extended);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех видеозаписей в соответствии с заданным критерием поиска.
        /// </summary>
        /// <param name="query">Строка поискового запроса.</param>
        /// <param name="sort">Вид сортировки.</param>
        /// <param name="isHd">Если true, то поиск производится только по видеозаписям высокого качества.</param>
        /// <param name="isAdult">Фильтр "Безопасный поиск".</param>
        /// <param name="filters">Список критериев, по которым требуется отфильтровать видео.</param>
        /// <param name="isSearchOwn">Искать по видеозаписям пользователя.</param>
        /// <returns>После успешного выполнения возвращает список объектов видеозаписей.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Video"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/video.search"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Video> SearchAll(string query, VideoSort sort, bool isHd = false, bool isAdult = false, VideoFilters filters = null, bool isSearchOwn = false)
        {
            const int count = 200;
            var i = 0;
            var result = new List<Video>();

            do
            {
                var currentItems = _video.Search(query, sort, isHd, isAdult, filters, isSearchOwn, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while ((++i * count < (_vk.CountFromLastResponse ?? 0)) && (i * count < 1000));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех видеозаписей, на которых отмечен пользователь.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>После успешного выполнения возвращает список объектов видеозаписей.</returns>
        /// <remarks>
        /// ЭТОТ МЕТОД ВЫБРАСЫВАЕТ ИСКЛЮЧЕНИЕ НА СЕРВЕРЕ ВК!!!
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Video"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/video.getUserVideos"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Video> GetAllUserVideos(long userId)
        {
            const int count = 100;
            var i = 0;
            var result = new List<Video>();

            do
            {
                var currentItems = _video.GetUserVideos(userId, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех альбомов видеозаписей пользователя или сообщества.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца альбомов (пользователь или сообщество). По умолчанию — идентификатор текущего 
        /// пользователя.</param>
        /// <param name="extended">true – позволяет получать поля <see cref="VideoAlbum.Count"/>, <see cref="VideoAlbum.Photo160"/> и 
        /// <see cref="VideoAlbum.Photo320"/> для каждого альбома.</param>
        /// <returns>
        /// После успешного выполнения возвращает массив объектов <see cref="VideoAlbum"/>, каждый из которых содержит следующие 
        /// поля: <see cref="VideoAlbum.Id"/>, <see cref="VideoAlbum.OwnerId"/> и <see cref="VideoAlbum.Title"/>.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Video"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/video.getAlbums"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<VideoAlbum> GetAllAlbums(long ownerId, bool extended = false)
        {
            const int count = 100;
            var i = 0;
            var result = new List<VideoAlbum>();

            do
            {
                var currentItems = _video.GetAlbums(ownerId, count, i * count, extended);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех комментариев к видеозаписи.
        /// </summary>
        /// <param name="videoId">Идентификатор видеозаписи.</param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит видеозапись.
        /// Обратите внимание, идентификатор сообщества в параметре <paramref name="ownerId"/> необходимо указывать со знаком "-" — например, 
        /// <paramref name="ownerId"/>=-1 соответствует идентификатору сообщества ВКонтакте API (club1).
        /// </param>
        /// <param name="needLikes">true — будет возвращено дополнительное поле <see cref="Comment.Likes"/>. По умолчанию поле <see cref="Comment.Likes"/> 
        /// не возвращается.</param>
        /// <param name="sort">Порядок сортировки комментариев.</param>
        /// <returns>После успешного выполнения возвращает общее количество комментариев и массив объектов <see cref="Comment"/>.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Video"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/video.getComments"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Comment> GetAllComments(long videoId, long? ownerId = null, bool needLikes = false, CommentsSort sort = null)
        {
            const int count = 100;
            var i = 0;
            var result = new List<Comment>();

            do
            {
                var currentItems = _video.GetComments(videoId, ownerId, needLikes, count, i * count, sort);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех видеозаписей, на которых есть непросмотренные отметки. 
        /// </summary>
        /// <returns>
        /// После успешного выполнения возвращает список объектов <see cref="Video"/> с дополнительным полем <see cref="Video.Tag"/>.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Video"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/video.getNewTags"/>.
        /// </remarks>        
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Video> GetAllNewTags()
        {
            const int count = 100;
            var i = 0;
            var allVideos = new List<Video>();

            do
            {
                var currentVideos = _video.GetNewTags(count, i * count);
                if (currentVideos != null) allVideos.AddRange(currentVideos);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return allVideos.ToReadOnlyCollection();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы с фотографиями.
    /// </summary>
    public class PhotoCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly PhotoCategory _photo;

        internal PhotoCategoryExtended(PhotoCategory photoCategory, VkApi vk)
        {
            _photo = photoCategory;
            _vk = vk;
        }

        /// <summary>
        /// Возвращает всех список альбомов пользователя или сообщества. 
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежат альбомы. Обратите внимание, идентификатор сообщества в параметре owner_id необходимо указывать со знаком &quot;-&quot; — например, owner_id=-1 соответствует идентификатору сообщества ВКонтакте API (club1)</param>
        /// <param name="albumIds">Перечисленные через запятую ID альбомов</param>
        /// <param name="needSystem">true – будут возвращены системные альбомы, имеющие отрицательные идентификаторы.</param>
        /// <param name="needCovers">true — будет возвращено дополнительное поле thumb_src. По умолчанию поле thumb_src не возвращается</param>
        /// <param name="photoSizes">true — будут возвращены размеры фотографий в специальном формате</param>
        /// <returns>Возвращает список объектов <see cref="PhotoAlbum"/></returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/photos.getAlbums"/>.
        /// </remarks>
        [ApiVersion("5.9")]
        public ReadOnlyCollection<PhotoAlbum> GetAllAlbums(long? ownerId = null, IEnumerable<long> albumIds = null, bool? needSystem = null, bool? needCovers = null, bool? photoSizes = null)
        {
            const int count = 50;
            var i = 0;
            var result = new List<PhotoAlbum>();

            do
            {
                var currentItems = _photo.GetAlbums(ownerId, albumIds, i * count, count, needSystem, needCovers, photoSizes);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
        
        /// <summary>
        /// Возвращает список всех фотографий в альбоме. 
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца альбома.
        /// <remarks>
        /// Обратите внимание, идентификатор сообщества в параметре owner_id необходимо указывать со знаком &quot;-&quot; — например, owner_id=-1 соответствует идентификатору сообщества ВКонтакте API (club1)  целое число, по умолчанию идентификатор текущего пользователя
        /// </remarks>
        /// </param>
        /// <param name="albumId">Идентификатор альбома. Для служебных альбомов используются следующие идентификаторы: 
        /// todo реализовать для следующих типов with PhotoAlbumType class
        /// wall — фотографии со стены; 
        /// profile — аватары; 
        /// saved — сохраненные фотографии. </param>
        /// <param name="photoIds">Идентификаторы фотографий, информацию о которых необходимо вернуть. список строк, разделенных через запятую</param>
        /// <param name="rev">Порядок сортировки фотографий (true — антихронологический, false — хронологический).</param>
        /// <param name="extended">True — будут возвращены дополнительные поля likes, comments, tags, can_comment. Поля comments и tags содержат только количество объектов. По умолчанию данные поля не возвращается.</param>
        /// <param name="feedType">Тип новости получаемый в поле type метода newsfeed.get, для получения только загруженных пользователем фотографий, либо только фотографий, на которых он был отмечен.</param>
        /// <param name="feed">Unixtime, который может быть получен методом newsfeed.get в поле date, для получения всех фотографий загруженных пользователем в определённый день либо на которых пользователь был отмечен. Также нужно указать параметр uid пользователя, с которым произошло событие. </param>
        /// <param name="photoSizes">Возвращать ли доступные размеры фотографии в специальном формате. флаг, может принимать значения 1 или 0</param>
        /// <returns>После успешного выполнения возвращает список объектов <see cref="Photo"/>.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/photos.get"/>.
        /// </remarks>
        [ApiMethodName("photos.get", Skip = true)]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Photo> GetAll(long? ownerId = null, long? albumId = null, IEnumerable<long> photoIds = null, bool? rev = null, bool? extended = null, PhotoFeedType feedType = null, DateTime? feed = null, bool? photoSizes = null)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<Photo>();

            do
            {
                var currentItems = _photo.Get(ownerId, albumId, photoIds, rev, extended, feedType, feed, photoSizes, i * count, count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех фотографий со страницы пользователя или сообщества. 
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, фотографии которого нужно получить. Обратите внимание, идентификатор сообщества в параметре owner_id необходимо указывать со знаком &quot;-&quot; — например, owner_id=-1 соответствует идентификатору сообщества ВКонтакте API (club1)</param>
        /// <param name="photoIds">Идентификаторы фотографий, информацию о которых необходимо вернуть</param>
        /// <param name="rev">порядок сортировки фотографий (1 — антихронологический, 0 — хронологический). флаг, может принимать значения 1 или 0</param>
        /// <param name="extended">1 — будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается. флаг, может принимать значения 1 или 0</param>
        /// <param name="feedType">Тип новости, получаемый в поле type метода newsfeed.get. строка</param>
        /// <param name="feed">Unixtime, который может быть получен методом newsfeed.get в поле date, для получения всех фотографий загруженных пользователем в определённый день либо на которых пользователь был отмечен. Также нужно указать параметр uid пользователя, с которым произошло событие</param>
        /// <param name="photoSizes">Возвращать ли размеры фотографий в специальном формате</param>
        /// <returns>После успешного выполнения возвращает массив объектов <see cref="Photo"/>. В случае, если запись на стене о том, что была обновлена фотография профиля, не удалена, будет возвращено дополнительное поле post_id, содержащее идентификатор записи на стене.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/photos.getProfile"/>.
        /// </remarks>
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Photo> GetAllProfile(long? ownerId = null, IEnumerable<long> photoIds = null, bool? rev = null, bool? extended = null, string feedType = null, DateTime? feed = null, bool? photoSizes = null)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<Photo>();

            do
            {
                var currentItems = _photo.GetProfile(ownerId, photoIds, rev, extended, feedType, feed, photoSizes, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Осуществляет поиск изображений по местоположению или описанию. 
        /// </summary>
        /// <param name="query">Строка поискового запроса</param>
        /// <param name="lat">Географическая широта отметки, заданная в градусах (от -90 до 90)</param>
        /// <param name="longitude">Географическая долгота отметки, заданная в градусах (от -180 до 180)</param>
        /// <param name="startTime">Время в формате unixtime, не раньше которого должны были быть загружены найденные фотографии. положительное число</param>
        /// <param name="endTime">Время в формате unixtime, не позже которого должны были быть загружены найденные фотографии. положительное число</param>
        /// <param name="sort">True – сортировать по количеству отметок "Мне нравится", false – сортировать по дате добавления фотографии. положительное число</param>
        /// <param name="radius">радиус поиска в метрах. (работает очень приближенно, поэтому реальное расстояние до цели может отличаться от заданного). Может принимать значения: 10, 100, 800, 6000, 50000 положительное число, по умолчанию 5000</param>
        /// <returns>После успешного выполнения возвращает список объектов фотографий.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/photos.search"/>.
        /// </remarks>
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Photo> SearchAll(string query = null, double? lat = null, double? longitude = null, DateTime? startTime = null, DateTime? endTime = null, bool? sort = null, int? radius = null)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<Photo>();

            do
            {
                var currentItems = _photo.Search(query, lat, longitude, startTime, endTime, sort, count, i * count, radius);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает все фотографии пользователя или сообщества в антихронологическом порядке. 
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, фотографии которого нужно получить. Обратите внимание, идентификатор сообщества в параметре owner_id необходимо указывать со знаком &quot;-&quot; — например, owner_id=-1 соответствует идентификатору сообщества ВКонтакте API (club1)</param>
        /// <param name="extended">True — возвращать расширенную информацию о фотографиях</param>
        /// <param name="photoSizes">True — будут возвращены размеры фотографий в специальном формате</param>
        /// <param name="noServiceAlbums">false — вернуть все фотографии, включая находящиеся в сервисных альбомах, таких как &quot;Фотографии на моей стене&quot; (по умолчанию);  true — вернуть фотографии только из стандартных альбомов пользователя или сообщества</param>
        /// <returns>После успешного выполнения возвращает список объектов <see cref="Photo"/>.
        /// <remarks>
        /// Если был задан параметр extended — будет возвращено поле likes: 
        /// user_likes: 1 — текущему пользователю нравится данная фотография, 0 - не указано.
        /// count — количество пользователей, которым нравится текущая фотография.
        /// 
        /// Если был задан параметр photo_sizes=1, вместо полей width и height возвращаются размеры копий фотографии в специальном формате.
        /// </remarks>
        ///</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/photos.getAll"/>.
        /// </remarks>
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Photo> GetAllEx(long? ownerId = null, bool? extended = null, bool? photoSizes = null, bool? noServiceAlbums = null)
        {
            const int count = 200;
            var i = 0;
            var result = new List<Photo>();

            do
            {
                var currentItems = _photo.GetAll(ownerId, extended, count, i * count, photoSizes, noServiceAlbums);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех фотографий, на которых отмечен пользователь 
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, список фотографий для которого нужно получить. положительное число, по умолчанию идентификатор текущего пользователя</param>
        /// <param name="extended">True — будут возвращены дополнительные поля likes, comments, tags, can_comment. Поля comments и tags содержат только количество объектов. По умолчанию данные поля не возвращается</param>
        /// <param name="sort">Сортировка результатов (false — по дате добавления отметки в порядке убывания, true — по дате добавления отметки в порядке возрастания)</param>
        /// <returns>После успешного выполнения возвращает список объектов photo. </returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/photos.getUserPhotos"/>.
        /// </remarks>
        [ApiMethodName("photos.getUserPhotos")]
        [VkValue("userId", 178964623)]
        [VkValue("count", 2)]
        [VkValue("offset", 3)]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Photo> GetAllUserPhotos(long? userId = null, bool? extended = null, bool? sort = null)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<Photo>();

            do
            {
                var currentItems = _photo.GetUserPhotos(userId, i * count, count, extended, sort);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех комментариев к фотографии. 
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит фотография. Обратите внимание, идентификатор сообщества в параметре owner_id необходимо указывать со знаком &quot;-&quot; — например, owner_id=-1 соответствует идентификатору сообщества ВКонтакте API (club1)</param>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <param name="needLikes">True — будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается</param>
        /// <param name="sort">Порядок сортировки комментариев (asc — от старых к новым, desc - от новых к старым)</param>
        /// <param name="accessKey">строка</param>
        /// <returns>После успешного выполнения возвращает список объектов <see cref="Comment"/>.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/photos.getComments"/>.
        /// </remarks>
        [ApiMethodName("photos.getComments")]
        [VkValue("owner_id", 1)]
        [VkValue("photo_id", 263219735)]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Comment> GetAllComments(long photoId, long? ownerId = null, bool? needLikes = null, CommentsSort sort = null, string accessKey = null)
        {
            const int count = 100;
            var i = 0;
            var result = new List<Comment>();

            do
            {
                var currentItems = _photo.GetComments(photoId, ownerId, needLikes, count, i * count, sort, accessKey);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает отсортированный в антихронологическом порядке список всех комментариев к конкретному альбому или ко всем альбомам пользователя. 
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежат фотографии. Обратите внимание, идентификатор сообщества в параметре owner_id необходимо указывать со знаком &quot;-&quot; — например, owner_id=-1 соответствует идентификатору сообщества ВКонтакте API (club1)</param>
        /// <param name="albumId">Идентификатор альбома. Если параметр не задан, то считается, что необходимо получить комментарии ко всем альбомам пользователя или сообщества</param>
        /// <param name="needLikes">True — будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается</param>
        /// <returns>После успешного выполнения возвращает список объектов <see cref="Comment"/>.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/photos.getAllComments"/>.
        /// </remarks>
        [ApiMethodName("photos.getAllComments", Skip = true)]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Comment> GetAllCommentsEx(long? ownerId = null, long? albumId = null, bool? needLikes = null)
        {
            const int count = 100;
            var i = 0;
            var result = new List<Comment>();

            do
            {
                var currentItems = _photo.GetAllComments(ownerId, albumId, needLikes, i * count, count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
        
        /// <summary>
        /// Возвращает список фотографий, на которых есть непросмотренные отметки. 
        /// </summary>
        /// <returns>После успешного выполнения возвращает список объектов <see cref="Photo"/>.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/photos.getNewTags"/>.
        /// </remarks>
        [ApiMethodName("photos.getNewTags", Skip = true)]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Photo> GetAllNewTags()
        {
            const int count = 100;
            var i = 0;
            var result = new List<Photo>();

            do
            {
                var currentItems = _photo.GetNewTags(i * count, count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
    }
}
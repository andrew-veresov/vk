using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы с аудиозаписями.
    /// </summary>
    public class AudioCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly AudioCategory _audio;

        internal AudioCategoryExtended(AudioCategory audioCategory, VkApi vk)
        {
            _audio = audioCategory;
            _vk = vk;
        }

        /// <summary>
        /// Возвращает список всех аудиозаписей группы.
        /// </summary>
        /// <param name="gid">Идентификатор группы, у которой необходимо получить аудиозаписи.</param>
        /// <param name="albumId">Идентификатор альбома, аудиозаписи которого необходимо вернуть (по умолчанию возвращаются аудиозаписи из всех альбомов).</param>
        /// <param name="aids">
        /// Список идентификаторов аудиозаписей группы, по которым необходимо получить информацию.
        /// Если список не указан (null), то ограничение на идентификаторы аудиозаписей на накладываются.
        /// </param>
        /// <returns>
        /// В случае успеха возвращает затребованный список аудиозаписей группы.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.get"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Audio> GetAllFromGroup(long gid, long? albumId = null, IEnumerable<long> aids = null)
        {
            const int count = 6000;
            var i = 0;
            var result = new List<Audio>();

            do
            {
                var currentItems = _audio.GetFromGroup(gid, albumId, aids, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while ((++i * count < (_vk.CountFromLastResponse ?? 0)) && (i * count < 6000));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех аудиозаписей пользователя и краткую информацию о нем.
        /// </summary>
        /// <param name="uid">Идентификатор пользователя, у которого необходимо получить аудиозаписи.</param>
        /// <param name="user">Базовая информация о владельце аудиозаписей - пользователе с идентификатором <paramref name="uid"/> (идентификатор, имя, фотография).</param>
        /// <param name="albumId">Идентификатор альбома пользователя, аудиозаписи которого необходимо получить (по умолчанию возвращаются аудиозаписи из всех альбомов).</param>
        /// <param name="aids">Список идентификаторов аудиозаписей пользователя, по которым необходимо получить информацию.</param>
        /// <returns>
        /// В случае успеха возвращает затребованный список аудиозаписей пользователя.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.get"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Audio> GetAll(long uid, out User user, long? albumId = null, IEnumerable<long> aids = null)
        {
            const int count = 6000;
            var i = 0;
            var result = new List<Audio>();

            do
            {
                var currentItems = _audio.Get(uid, out user, albumId, aids, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while ((++i * count < (_vk.CountFromLastResponse ?? 0)) && (i * count < 6000));

            return result.ToReadOnlyCollection();
        }
        
        /// <summary>
        /// Возвращает список всех аудиозаписей пользователя.
        /// </summary>
        /// <param name="uid">Идентификатор пользователя, у которого необходимо получить аудиозаписи.</param>
        /// <param name="albumId">Идентификатор альбома пользователя, аудиозаписи которого необходимо получить (по умолчанию возвращаются аудиозаписи из всех альбомов).</param>
        /// <param name="aids">Список идентификаторов аудиозаписей пользователя, по которым необходимо получить информацию.</param>
        /// <returns>В случае успеха возвращает затребованный список аудиозаписей пользователя.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.get"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Audio> GetAll(long uid, long? albumId = null, IEnumerable<long> aids = null)
        {
            const int count = 6000;
            var i = 0;
            var result = new List<Audio>();

            do
            {
                var currentItems = _audio.Get(uid, albumId, aids, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while ((++i * count < (_vk.CountFromLastResponse ?? 0)) && (i * count < 6000));

            return result.ToReadOnlyCollection();
        }
        
        /// <summary>
        /// Возвращает список всех аудиозаписей в соответствии с заданным критерием поиска.
        /// </summary>
        /// <param name="query">Cтрока поискового запроса</param>
        /// <param name="totalCount">Общее количество аудиозаписей удовлетворяющих запросу</param>
        /// <param name="autoComplete">Если этот параметр равен true, возможные ошибки в поисковом запросе будут исправлены. Например, при поисковом запросе <strong>Иуфдуы</strong> поиск будет осуществляться по строке <strong>Beatles</strong></param>
        /// <param name="sort">Вид сортировки</param>
        /// <param name="findLyrics">Будет ли производиться только по тем аудиозаписям, которые содержат тексты.</param>
        /// <returns>Список объектов класса Audio.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.search"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Audio> SearchAll(
            string query,
            out int totalCount,
            bool? autoComplete = null,
            AudioSort? sort = null,
            bool? findLyrics = null)
        {
            const int count = 300;
            var i = 0;
            var result = new List<Audio>();

            do
            {
                var currentItems = _audio.Search(query, out totalCount, autoComplete, sort, findLyrics, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while ((++i * count < (_vk.CountFromLastResponse ?? 0)) && (i * count < 1000));

            return result.ToReadOnlyCollection();
        }
        
        /// <summary>
        /// Возвращает список всех аудиозаписей из раздела "Популярное".
        /// </summary>
        /// <param name="onlyEng">true – возвращать только зарубежные аудиозаписи. false – возвращать все аудиозаписи. (по умолчанию) </param>
        /// <param name="genre">идентификатор жанра </param>
        /// <returns>Список аудиозаписей из раздела "Популярное"</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getPopular"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Audio> GetAllPopular(bool onlyEng = false, AudioGenre? genre = null)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<Audio>();

            do
            {
                var currentItems = _audio.GetPopular(onlyEng, genre, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех альбомов аудиозаписей пользователя или группы.
        /// </summary>
        /// <param name="ownerid">Идентификатор пользователя или сообщества, у которого необходимо получить список альбомов с аудио.</param>
        /// <returns>
        /// После успешного выполнения возвращает массив альбомов аудиоальбомов <see cref="AudioAlbum"/>.
        /// </returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getAlbums"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<AudioAlbum> GetAllAlbums(long ownerid)
        {
            const int count = 100;
            var i = 0;
            var result = new List<AudioAlbum>();

            do
            {
                var currentItems = _audio.GetAlbums(ownerid, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех рекомендуемых аудиозаписей на основе списка воспроизведения заданного пользователя или на основе одной выбранной аудиозаписи.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя для получения списка рекомендаций на основе его набора аудиозаписей (по умолчанию — идентификатор 
        /// текущего пользователя).</param>
        /// <param name="shuffle">true — включен случайный порядок.</param>
        /// <param name="targetAudio">Идентификатор аудиозаписи, на основе которой будет строиться список рекомендаций. Используется вместо параметра uid. 
        /// Идентификатор представляет из себя разделённые знаком подчеркивания id пользователя, которому принадлежит аудиозапись, и id самой аудиозаписи. 
        /// Если аудиозапись принадлежит сообществу, то в качестве первого параметра используется -id сообщества.</param>
        /// <returns>Список рекомендуемых аудиозаписей.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getRecommendations"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Audio> GetAllRecommendations(long? userId = null, bool shuffle = true, string targetAudio = "")
        {
            const int count = 1000;
            var i = 0;
            var result = new List<Audio>();

            do
            {
                var currentItems = _audio.GetRecommendations(userId, count, i * count, shuffle, targetAudio);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
    }
}
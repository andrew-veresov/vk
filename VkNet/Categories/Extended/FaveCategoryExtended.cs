using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы с закладками.
    /// </summary>
    public class FaveCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly FaveCategory _fave;

        internal FaveCategoryExtended(FaveCategory faveCategory, VkApi vk)
        {
            _fave = faveCategory;
            _vk = vk;
        }

        /// <summary>
        /// Возвращает список всех пользователей, добавленных текущим пользователем в закладки.
        /// </summary>
        /// <returns>После успешного выполнения возвращает список объектов пользователей.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/fave.getUsers"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<User> GetAllUsers()
        {
            const int count = 50;
            var i = 0;
            var allUsers = new List<User>();

            do
            {
                var currentUsers = _fave.GetUsers(count, i * count);
                if (currentUsers != null) allUsers.AddRange(currentUsers);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return allUsers.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает все фотографии, на которых текущий пользователь поставил отметку "Мне нравится".
        /// </summary>
        /// <returns>После успешного выполнения возвращает список объектов фотографий.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/fave.getPhotos"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Photo> GetAllPhotos()
        {
            const int count = 50;
            var i = 0;
            var allPhotos = new List<Photo>();

            do
            {
                var currentPhotos = _fave.GetPhotos(count, i * count);
                if (currentPhotos != null) allPhotos.AddRange(currentPhotos);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return allPhotos.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает все записи, на которых текущий пользователь поставил отметку "Мне нравится".
        /// </summary>
        /// <returns>После успешного выполнения возвращает список объектов записей на стене.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/fave.getPosts"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Post> GetAllPosts()//, bool extended = false)
        {
            const int count = 100;
            var i = 0;
            var allPosts = new List<Post>();

            do
            {
                var currentPosts = _fave.GetPosts(count, i * count);
                if (currentPosts != null) allPosts.AddRange(currentPosts);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return allPosts.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех видеозаписей, на которых текущий пользователь поставил отметку "Мне нравится".
        /// </summary>
        /// <returns>После успешного выполнения возвращает список объектов записей на стене.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/fave.getVideos"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<Video> GetAllVideos()
        {
            const int count = 50;
            var i = 0;
            var allVideos = new List<Video>();

            do
            {
                var currentVideos = _fave.GetVideos(count, i * count);
                if (currentVideos != null) allVideos.AddRange(currentVideos);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return allVideos.ToReadOnlyCollection();
        }


        /// <summary>
        /// Возвращает все ссылки, добавленные в закладки текущим пользователем.
        /// </summary>
        /// <returns>После успешного выполнения возвращает массив объектов Link.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/fave.getLinks"/>.
        /// </remarks>
        [Pure]
        [ApiVersion("5.9")]
        public ReadOnlyCollection<ExternalLink> GetAllLinks()
        {
            const int count = 50;
            var i = 0;
            var allExternalLinks = new List<ExternalLink>();

            do
            {
                var currentLinks = _fave.GetLinks(count, i * count);
                if (currentLinks != null) allExternalLinks.AddRange(currentLinks);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return allExternalLinks.ToReadOnlyCollection();
        }
    }
}
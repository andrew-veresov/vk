using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы со справочной информацией.
    /// </summary>
    public class DatabaseCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly DatabaseCategory _database;

        internal DatabaseCategoryExtended(DatabaseCategory databaseCategory, VkApi vk)
        {
            _database = databaseCategory;
            _vk = vk;
        }
        
        /// <summary>
        /// Возвращает список всех стран.
        /// </summary>
        /// <param name="needAll">Флаг - вернуть список всех стран.</param>
        /// <param name="codes">Перечисленные через запятую двухбуквенные коды стран в стандарте ISO 3166-1 alpha-2 
        /// <see href="http://vk.com/dev/country_codes"/>.</param>
        /// <remarks>
        /// Если не заданы параметры needAll и code, то возвращается краткий список стран, расположенных наиболее близко к стране 
        /// текущего пользователя. Если задан параметр needAll, то будет возвращен список всех стран. Если задан параметр code, 
        /// то будут возвращены только страны с перечисленными ISO 3166-1 alpha-2 кодами.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/database.getCountries"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Country> GetAllCountries(bool needAll = true, string codes = "")
        {
            const int count = 1000;
            var i = 0;
            var result = new List<Country>();

            do
            {
                var currentItems = _database.GetCountries(needAll, codes, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех регионов.
        /// </summary>
        /// <param name="countryId">Идентификатор страны.</param>
        /// <param name="query">Строка поискового запроса.</param>
        /// <returns>Список регионов.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/database.getRegions"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Region> GetAllRegions(int countryId, string query = "")
        {
            const int count = 1000;
            var i = 0;
            var result = new List<Region>();

            do
            {
                var currentItems = _database.GetRegions(countryId, query, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех городов.
        /// </summary>
        /// <param name="countryId">Идентификатор страны.</param>
        /// <param name="regionId">Идентификатор региона.</param>
        /// <param name="query">Строка поискового запроса. Например, Санкт.</param>
        /// <param name="needAll">true – возвращать все города. false – возвращать только основные города.</param>
        /// <returns>Cписок городов</returns>
        /// <remarks>
        /// Возвращает коллекцию городов, каждый из которых содержит поля <see cref="City.Id"/> и <see cref="City.Title"/>. 
        /// При наличии информации о регионе и/или области, в которых находится данный город, в объекте могут дополнительно 
        /// включаться поля <see cref="City.Area"/> и <see cref="City.Region"/>. 
        /// Если не задан параметр <paramref name="query"/>, то будет возвращен список самых крупных городов в заданной стране. 
        /// Если задан параметр <paramref name="query"/>, то будет возвращен список городов, которые релевантны поисковому запросу.
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/database.getCities"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<City> GetAllCities(int countryId, int? regionId = null, string query = "", bool? needAll = false)
        {
            const int count = 1000;
            var i = 0;
            var result = new List<City>();

            do
            {
                var currentItems = _database.GetCities(countryId, regionId, query, needAll, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех высших учебных заведений.
        /// </summary>
        /// <param name="countryId">Идентификатор страны, учебные заведения которой необходимо вернуть.</param>
        /// <param name="cityId">Идентификатор города, учебные заведения которого необходимо вернуть.</param>
        /// <param name="query">Строка поискового запроса. Например, СПБ.</param>
        /// <returns>Список высших учебных заведений, удовлетворяющих заданным условиям.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/database.getUniversities"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<University> GetAllUniversities(int countryId, int cityId, string query = "")
        {
            const int count = 10000;
            var i = 0;
            var result = new List<University>();

            do
            {
                var currentItems = _database.GetUniversities(countryId, cityId, query, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех школ.
        /// </summary>
        /// <param name="countryId">Идентификатор страны, школы которой необходимо вернуть.</param>
        /// <param name="cityId">Идентификатор города, школы которого необходимо вернуть.</param>
        /// <param name="query">Строка поискового запроса. Например, гимназия.</param>
        /// <returns>Cписок школ.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/database.getSchools"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<School> GetAllSchools(int countryId, int cityId, string query = "")
        {
            const int count = 10000;
            var i = 0;
            var result = new List<School>();

            do
            {
                var currentItems = _database.GetSchools(countryId, cityId, query, i * count, count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }

        /// <summary>
        /// Возвращает список всех факультетов.
        /// </summary>
        /// <param name="universityId">Идентификатор университета, факультеты которого необходимо получить.</param>
        /// <returns>Список факультетов.</returns>
        /// <remarks>
        /// Страница документации ВКонтакте <see href="http://vk.com/dev/database.getFaculties"/>.
        /// </remarks>
        [Pure]
        public ReadOnlyCollection<Faculty> GetAllFaculties(long universityId)
        {
            const int count = 10000;
            var i = 0;
            var result = new List<Faculty>();

            do
            {
                var currentItems = _database.GetFaculties(universityId, count, i * count);
                if (currentItems != null) result.AddRange(currentItems);
            } while (++i * count < (_vk.CountFromLastResponse ?? 0));

            return result.ToReadOnlyCollection();
        }
    }
}
namespace VkNet
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Класс предоставляет некоторые полезные в работе методы расширения.
    /// </summary>
    public static class VkApiExtensions
    {
        /// <summary>
        /// Делит коллекцию на несколько частей.
        /// </summary>
        /// <param name="source">Коллекция, которую нужно разделить на части.</param>
        /// <param name="parts">Количество частей, на которые необходимо разделить коллекцию.</param>
        /// <typeparam name="T" />
        /// <returns>Возвращает коллекцию коллекций.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int parts)
        {
            var i = 0;
            var splits = from item in source
                group item by i++ % parts into part
                select part.AsEnumerable();
            return splits;
        }

        /// <summary>
        /// Разбивает коллекцию на блоки определенного размера.
        /// </summary>
        /// <param name="source">Коллекция, которую нужно разделить на блоки.</param>
        /// <param name="chunkSize">Максимальный размер блока, используемых при делении коллекции..</param>
        /// <typeparam name="T" />
        /// <returns>Возвращает коллекцию коллекций.</returns>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            while (source.Any())
            {
                yield return source.Take(chunkSize);
                source = source.Skip(chunkSize);
            }
        }
    }
}
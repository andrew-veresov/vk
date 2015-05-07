namespace VkNet
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ����� ������������� ��������� �������� � ������ ������ ����������.
    /// </summary>
    public static class VkApiExtensions
    {
        /// <summary>
        /// ����� ��������� �� ��������� ������.
        /// </summary>
        /// <param name="source">���������, ������� ����� ��������� �� �����.</param>
        /// <param name="parts">���������� ������, �� ������� ���������� ��������� ���������.</param>
        /// <typeparam name="T" />
        /// <returns>���������� ��������� ���������.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int parts)
        {
            var i = 0;
            var splits = from item in source
                group item by i++ % parts into part
                select part.AsEnumerable();
            return splits;
        }

        /// <summary>
        /// ��������� ��������� �� ����� ������������� �������.
        /// </summary>
        /// <param name="source">���������, ������� ����� ��������� �� �����.</param>
        /// <param name="chunkSize">������������ ������ �����, ������������ ��� ������� ���������..</param>
        /// <typeparam name="T" />
        /// <returns>���������� ��������� ���������.</returns>
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
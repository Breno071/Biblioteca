using System.Diagnostics.CodeAnalysis;

namespace API.Shared.Extensions
{
    public static class EnumerableExtensionsMethods
    {
        public static bool IsCollectionNotEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? list)
        {
            return !list.IsCollectionEmpty();
        }

        public static bool IsCollectionEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? list)
        {
            return list?.Any() != true;
        }

        public static bool In<T>(this T item, params T[]? itens)
        {
            return itens?.Contains(item) == true;
        }

        public static bool In<T>(this T item, IEnumerable<T>? itens)
        {
            return itens?.Contains(item) == true;
        }

        public static bool NotIn<T>(this T item, params T[]? itens)
        {
            return itens?.Contains(item) != true;
        }

        public static bool NotIn<T>(this T item, IEnumerable<T>? itens)
        {
            return itens?.Contains(item) != true;
        }
    }
}

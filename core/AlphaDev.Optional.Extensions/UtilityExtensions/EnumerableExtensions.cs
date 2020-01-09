using System.Collections;

namespace AlphaDev.Optional.Extensions.UtilityExtensions
{
    public static class EnumerableExtensions
    {
        public static bool Any(this IEnumerable enumerable) => enumerable.GetEnumerator().MoveNext();
    }
}
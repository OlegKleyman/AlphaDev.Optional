using System;
using System.Collections;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class EnumerableExtensions
    {
        public static Option<TEnumerable> SomeNotEmpty<TEnumerable>(this TEnumerable enumerable)
            where TEnumerable : IEnumerable
        {
            return enumerable.SomeWhen(x => x.GetEnumerator().MoveNext());
        }

        public static Option<TEnumerable, TException> SomeNotEmpty<TEnumerable, TException>(
            this TEnumerable enumerable, Func<TException> exceptionFactory)
            where TEnumerable : IEnumerable
        {
            return enumerable.SomeWhen(x => x.GetEnumerator().MoveNext(), exceptionFactory);
        }
    }
}
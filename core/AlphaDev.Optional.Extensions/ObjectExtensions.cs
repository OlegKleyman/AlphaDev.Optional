using System;
using System.Collections;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class ObjectExtensions
    {
        public static Option<T> SomeWhenNotNull<T>(this T? target) where T : class => target.SomeNotNull()!;

        public static Option<T, TException> SomeWhenNotNull<T, TException>(this T? target,
            Func<TException> exceptionFactory) where T : class =>
            target.SomeWhenNotNull().WithException(exceptionFactory);

        public static Option<T> SomeNotEmpty<T>(this T target, Func<T, IEnumerable> getEnumerable)
        {
            return target.SomeWhen(arg => getEnumerable(arg).GetEnumerator().MoveNext());
        }

        public static Option<T, TException> SomeNotEmpty<T, TException>(this T target,
            Func<T, IEnumerable> getEnumerable, Func<T, TException> exceptionFactory)
        {
            return target.SomeWhen(arg => getEnumerable(arg).GetEnumerator().MoveNext(), exceptionFactory);
        }
    }
}
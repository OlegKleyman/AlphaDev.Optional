using System;
using System.Collections;
using System.Threading.Tasks;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class TaskEnumerableExtensions
    {
        public static async Task<Option<TEnumerable>> SomeNotEmptyAsync<TEnumerable>(
            this Task<TEnumerable> task)
            where TEnumerable : IEnumerable
        {
            return (await task).SomeWhen(enumerable => enumerable.GetEnumerator().MoveNext());
        }

        public static async Task<Option<TEnumerable, TException>> SomeNotEmptyAsync<TEnumerable, TException>(
            this Task<TEnumerable> task, Func<TException> exceptionFactory)
            where TEnumerable : IEnumerable
        {
            return (await task).SomeWhen(enumerable => enumerable.GetEnumerator().MoveNext(), exceptionFactory);
        }
    }
}
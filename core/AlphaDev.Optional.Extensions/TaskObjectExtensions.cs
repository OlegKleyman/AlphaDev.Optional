using System;
using System.Collections;
using System.Threading.Tasks;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class TaskObjectExtensions
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

        public static async Task<Option<T>> SomeNotNullAsync<T>(this Task<T> task) => (await task).SomeNotNull();

        public static async Task<Option<T, TException>> SomeNotNullAsync<T, TException>(this Task<T> task,
            Func<TException> exceptionFactory) => (await task).SomeNotNull(exceptionFactory);

        public static async Task<Option<T>> SomeAsync<T>(this Task<T> task) => (await task).Some();

        public static async Task<Option<T, TException>> SomeAsync<T, TException>(this Task<T> task) =>
            (await task).Some<T, TException>();


        public static Task<Option<T>> NoneAsync<T>(
#pragma warning disable IDE0060 // Remove unused parameter - this is purposely unused for a convenience method
            this Task<T> task
#pragma warning restore IDE0060 // Remove unused parameter
        ) => Task.FromResult(Option.None<T>());

        public static async Task<Option<T, TException>> NoneAsync<T, TException>(this Task<T> task,
            TException exception) => (await task).None(exception);
    }
}
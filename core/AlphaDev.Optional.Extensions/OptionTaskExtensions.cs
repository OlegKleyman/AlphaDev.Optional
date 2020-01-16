using System;
using System.Collections;
using System.Threading.Tasks;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class OptionTaskExtensions
    {
        public static async Task MatchSomeAsync<T, TException>(this Task<Option<T, TException>> optionTask,
            Func<T, Task> some)
        {
            await (await optionTask).MatchSomeAsync(some);
        }

        public static async Task MatchSomeAsync<T, TException>(this Task<Option<T, TException>> optionTask,
            Action<T> some) => (await optionTask).MatchSome(some);

        [Obsolete("Use ValueOrExceptionAsync instead.")]
        public static async Task<T> GetValueOrExceptionAsync<T, TException>(this Task<Option<T, TException>> option)
            where TException : T
        {
            return (await option).ValueOr(x => x);
        }

        public static async Task<T> ValueOrExceptionAsync<T, TException>(this Task<Option<T, TException>> option)
            where TException : T
        {
            return (await option).ValueOr(x => x);
        }

        public static async Task<TException> ExceptionOrValueAsync<T, TException>(
            this Task<Option<T, TException>> option)
            where T : TException
        {
            return (await option).Map(value => (TException) value).ValueOr(exception => exception);
        }

        public static async Task<T> ValueOrAsync<T, TException>(this Task<Option<T, TException>> option,
            Func<TException, T> exception) => (await option).ValueOr(exception);

        public static async Task<Option<T>> NotEmptyAsync<T>(this Task<Option<T>> task) where T : IEnumerable =>
            (await task).NotEmpty();

        public static async Task<Option<T, TException>> NotEmptyAsync<T, TException>(
            this Task<Option<T, TException>> task, Func<TException> exceptionFactory) where T : IEnumerable =>
            (await task).NotEmpty(exceptionFactory);

        public static async Task<TResult> MatchAsync<TValue, TException, TResult>(
            this Task<Option<TValue, TException>> task, Func<TValue, TResult> some, Func<TException, TResult> none) =>
            (await task).Match(some, none);

        public static async Task<TResult> MatchAsync<TValue, TResult>(this Task<Option<TValue>> task,
            Func<TValue, TResult> some, Func<TResult> none) => (await task).Match(some, none);

        public static async Task MatchAsync<TValue, TException>(this Task<Option<TValue, TException>> task,
            Action<TValue> some, Action<TException> none) => (await task).Match(some, none);

        public static async Task MatchAsync<TValue>(this Task<Option<TValue>> task, Action<TValue> some, Action none) =>
            (await task).Match(some, none);

        public static async Task MatchSomeAsync<T>(this Task<Option<T>> task, Action<T> some) =>
            (await task).MatchSome(some);

        public static async Task MatchSomeAsync<T>(this Task<Option<T>> task, Func<T, Task> some)
        {
            await (await task).MatchSomeAsync(some);
        }

        public static async Task MatchNoneAsync<T>(this Task<Option<T>> task, Action none) =>
            (await task).MatchNone(none);

        public static async Task MatchNoneAsync<T>(this Task<Option<T>> task, Func<Task> none)
        {
            await (await task).MatchNoneAsync(none);
        }

        public static async Task MatchNoneAsync<T, TException>(this Task<Option<T, TException>> task,
            Action<TException> none) => (await task).MatchNone(none);

        public static async Task MatchNoneAsync<T, TException>(this Task<Option<T, TException>> task,
            Func<TException, Task> none)
        {
            await (await task).MatchNoneAsync(none);
        }
    }
}
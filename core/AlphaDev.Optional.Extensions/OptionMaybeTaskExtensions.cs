using System;
using System.Collections;
using System.Threading.Tasks;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class OptionMaybeTaskExtensions
    {
        public static async Task<Option<T>> NotEmptyAsync<T>(this Task<Option<T>> task) where T : IEnumerable =>
            (await task).NotEmpty();

        public static async Task<TResult> MatchAsync<TValue, TResult>(this Task<Option<TValue>> task,
            Func<TValue, TResult> some, Func<TResult> none) => (await task).Match(some, none);

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
    }
}
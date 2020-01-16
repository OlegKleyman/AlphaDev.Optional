using System;
using System.Collections;
using System.Threading.Tasks;
using AlphaDev.Optional.Extensions.UtilityExtensions;
using Optional;
using Optional.Async;
using Optional.Unsafe;

namespace AlphaDev.Optional.Extensions
{
    public static class OptionExtensions
    {
        public static Option<T> FilterNotNull<T>(this Option<T> option) => option.NotNull()!;

        [Obsolete("Use ValueOrException instead.")]
        public static T GetValueOrException<T, TException>(this Option<T, TException> option) where TException : T
        {
            return option.ValueOr(x => x);
        }

        public static T ValueOrException<T, TException>(this Option<T, TException> option) where TException : T
        {
            return option.ValueOr(x => x);
        }

        public static TException ExceptionOrValue<T, TException>(this Option<T, TException> option) where T : TException
        {
            return option.Map(value => (TException) value).ValueOr(x => x);
        }

        public static async Task MatchSomeAsync<T, TException>(this Option<T, TException> option, Func<T, Task> some)
        {
            await option.Map(some).ValueOr(() => Task.CompletedTask);
        }

        public static async Task MatchSomeAsync<T>(this Option<T> option, Func<T, Task> some)
        {
            await option.Map(some).ValueOr(() => Task.CompletedTask);
        }

        public static async Task MatchNoneAsync<T>(this Option<T> option, Func<Task> none)
        {
            await option.Map(_ => Task.CompletedTask).ValueOr(none);
        }

        public static async Task MatchNoneAsync<T, TException>(this Option<T, TException> option, Func<TException, Task> none)
        {
            await option.Map(_ => Task.CompletedTask).ValueOr(none);
        }

        public static async Task MatchAsync<T, TException>(this Option<T, TException> option,
            Func<T, Task> some, Func<TException, Task> none)
        {
            await option.Match(some, none);
        }

        public static Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult, TExceptionResult>(
            this Option<T, TException> option, Func<T, Task<Option<TResult, TExceptionResult>>> mapping,
            Func<TExceptionResult, TException> exceptionMapping)
        {
            return option.FlatMapAsync(arg => mapping(arg).MapExceptionAsync(exceptionMapping));
        }

        public static Option<T> NotEmpty<T>(this Option<T> option) where T : IEnumerable
        {
            return option.Filter(enumerable => enumerable.Any());
        }

        public static Option<T, TException> NotEmpty<T, TException>(this Option<T, TException> option,
            Func<TException> exceptionFactory) where T : IEnumerable
        {
            return option.Filter(enumerable => enumerable.Any(), exceptionFactory);
        }
    }
}
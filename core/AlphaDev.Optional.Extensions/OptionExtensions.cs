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

        public static T GetValueOrException<T, TException>(this Option<T, TException> option) where TException : T
        {
            return option.ValueOr(x => x);
        }

        public static async Task MatchSomeAsync<T, TException>(this Option<T, TException> option, Func<T, Task> some)
        {
            if (option.HasValue)
            {
                await some(option.ValueOrFailure());
            }
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
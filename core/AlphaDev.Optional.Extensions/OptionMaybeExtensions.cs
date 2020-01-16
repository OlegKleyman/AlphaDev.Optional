using System;
using System.Collections;
using System.Threading.Tasks;
using AlphaDev.Optional.Extensions.UtilityExtensions;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class OptionMaybeExtensions
    {
        public static Option<T> FilterNotNull<T>(this Option<T> option) => option.NotNull()!;

        public static async Task MatchSomeAsync<T>(this Option<T> option, Func<T, Task> some)
        {
            await option.Map(some).ValueOr(() => Task.CompletedTask);
        }

        public static async Task MatchNoneAsync<T>(this Option<T> option, Func<Task> none)
        {
            await option.Map(_ => Task.CompletedTask).ValueOr(none);
        }

        public static Option<T> NotEmpty<T>(this Option<T> option) where T : IEnumerable
        {
            return option.Filter(enumerable => enumerable.Any());
        }
    }
}

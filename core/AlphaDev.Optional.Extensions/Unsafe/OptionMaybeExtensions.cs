using System;
using Optional;

namespace AlphaDev.Optional.Extensions.Unsafe
{
    public static class OptionMaybeExtensions
    {
        public static TValue ValueOrFailure<TValue>(this Option<TValue> option, Action fail)
        {
            return option.ValueOr(() =>
            {
                fail();
                throw new InvalidOperationException("Option is none.");
            });
        }
    }
}

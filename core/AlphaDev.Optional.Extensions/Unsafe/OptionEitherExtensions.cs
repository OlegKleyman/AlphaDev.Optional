using System;
using Optional;

namespace AlphaDev.Optional.Extensions.Unsafe
{
    public static class OptionEitherExtensions
    {
        public static TException ExceptionOrFailure<T, TException>(this Option<T, TException> option)
        {
            return option.Match(_ => throw new InvalidOperationException("Option has some."),
                exception => exception);
        }

        public static TException ExceptionOrFailure<TValue, TException>(this Option<TValue, TException> option,
            Action fail)
        {
            return option.Match(value =>
            {
                fail();
                throw new InvalidOperationException("Option has some.");
            }, exception => exception);
        }

        public static TValue ValueOrFailure<TValue, TException>(this Option<TValue, TException> option, Action fail)
        {
            return option.ValueOr(() =>
            {
                fail();
                throw new InvalidOperationException("Option is none.");
            });
        }
    }
}
using System;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit.Unsafe
{
    public class OptionMaybeExtensionsTests
    {
        [Fact]
        public static void ValueOrFailureWithActionExecutesFailureActionWhenOptionIsNone()
        {
            var executed = false;
            try
            {
                Option.None<string>().ValueOrFailure(() => executed = true);
            }
            catch (InvalidOperationException)
            {
                executed.Should().BeTrue();
            }
        }

        [Fact]
        public static void ValueOrFailureWithActionReturnsValueWhenOptionHasSome()
        {
            "value".Some().ValueOrFailure(() => { }).Should().Be("value");
        }

        [Fact]
        public static void ValueOrFailureWithActionThrowsInvalidOperationValueWhenOptionHasNone()
        {
            Action valueOrFailure = () => Option.None<string>().ValueOrFailure(() => { });
            valueOrFailure.Should().Throw<InvalidOperationException>().WithMessage("Option is none.");
        }
    }
}

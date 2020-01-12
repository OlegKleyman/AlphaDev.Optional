using System;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit.Unsafe
{
    public static class OptionEitherExtensionsTests
    {
        [Fact]
        public static void ExceptionOrFailureReturnsExceptionWhenOptionIsNone()
        {
            Option.None<string, string>("ex").ExceptionOrFailure().Should().Be("ex");
        }

        [Fact]
        public static void ExceptionOrFailureThrowsInvalidOperationExceptionWhenOptionHasSome()
        {
            Action exceptionOrFailure = () => Option.Some<string?, string>(default).ExceptionOrFailure();
            exceptionOrFailure.Should().Throw<InvalidOperationException>().WithMessage("Option has some.");
        }

        [Fact]
        public static void ExceptionOrFailureWithActionAndArgumentExecutesFailureActionWhenOptionHasSome()
        {
            string? result = null;
            try
            {
                Option.Some<string, string>("test").ExceptionOrFailure(s => result = s);
            }
            catch (InvalidOperationException)
            {
                result.Should().Be("test");
            }
        }

        [Fact]
        public static void ExceptionOrFailureWithActionAndArgumentReturnsExceptionWhenOptionIsNone()
        {
            Option.None<string, string>("ex").ExceptionOrFailure(_ => { }).Should().Be("ex");
        }

        [Fact]
        public static void ExceptionOrFailureWithActionAndArgumentThrowsInvalidOperationExceptionWhenOptionHasSome()
        {
            Action exceptionOrFailure = () => Option.Some<string?, string>(default).ExceptionOrFailure(_ => { });
            exceptionOrFailure.Should().Throw<InvalidOperationException>().WithMessage("Option has some.");
        }

        [Fact]
        public static void ExceptionOrFailureWithActionExecutesFailureActionWhenOptionHasSome()
        {
            var executed = false;
            try
            {
                Option.Some<string?, string>(default).ExceptionOrFailure(() => executed = true);
            }
            catch (InvalidOperationException)
            {
                executed.Should().BeTrue();
            }
        }

        [Fact]
        public static void ExceptionOrFailureWithActionReturnsExceptionWhenOptionIsNone()
        {
            Option.None<string, string>("ex").ExceptionOrFailure(() => { }).Should().Be("ex");
        }

        [Fact]
        public static void ExceptionOrFailureWithActionThrowsInvalidOperationExceptionWhenOptionHasSome()
        {
            Action exceptionOrFailure = () => Option.Some<string?, string>(default).ExceptionOrFailure(() => { });
            exceptionOrFailure.Should().Throw<InvalidOperationException>().WithMessage("Option has some.");
        }

        [Fact]
        public static void ValueOrFailureWithActionAndArgumentExecutesFailureActionWhenOptionIsNone()
        {
            string? result = null;
            try
            {
                Option.None<string, string>("test").ValueOrFailure(x => result = x);
            }
            catch (InvalidOperationException)
            {
                result.Should().Be("test");
            }
        }

        [Fact]
        public static void ValueOrFailureWithActionAndArgumentReturnsValueWhenOptionHasSome()
        {
            Option.Some<string, string>("value").ValueOrFailure(_ => { }).Should().Be("value");
        }

        [Fact]
        public static void ValueOrFailureWithActionAndArgumentThrowsInvalidOperationValueWhenOptionHasNone()
        {
            Action valueOrFailure = () => Option.None<string, string?>(default).ValueOrFailure(_ => { });
            valueOrFailure.Should().Throw<InvalidOperationException>().WithMessage("Option is none.");
        }

        [Fact]
        public static void ValueOrFailureWithActionExecutesFailureActionWhenOptionIsNone()
        {
            var executed = false;
            try
            {
                Option.None<string, string?>(default).ValueOrFailure(() => executed = true);
            }
            catch (InvalidOperationException)
            {
                executed.Should().BeTrue();
            }
        }

        [Fact]
        public static void ValueOrFailureWithActionReturnsValueWhenOptionHasSome()
        {
            Option.Some<string, string>("value").ValueOrFailure(() => { }).Should().Be("value");
        }

        [Fact]
        public static void ValueOrFailureWithActionThrowsInvalidOperationValueWhenOptionHasNone()
        {
            Action valueOrFailure = () => Option.None<string, string?>(default).ValueOrFailure(() => { });
            valueOrFailure.Should().Throw<InvalidOperationException>().WithMessage("Option is none.");
        }
    }
}
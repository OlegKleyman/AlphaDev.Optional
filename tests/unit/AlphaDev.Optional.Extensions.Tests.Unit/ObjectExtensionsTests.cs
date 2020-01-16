using System;
using System.Linq;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void SomeNotEmptyEitherReturnsNoneWhenEnumerableIsEmpty()
        {
            1.SomeNotEmpty(arg => Array.Empty<object>(), o => o.ToString())
             .ExceptionOrFailure()
             .Should()
             .Be("1");
        }

        [Fact]
        public void SomeNotEmptyEitherReturnsSomeWheEnumerableIsNotEmpty()
        {
            1.SomeNotEmpty(i => Enumerable.Repeat(1, 1), i => default(object))
             .ValueOrFailure()
             .Should()
             .Be(1);
        }

        [Fact]
        public void SomeNotEmptyReturnsNoneWhenEnumerableIsEmpty()
        {
            default(object).SomeNotEmpty(arg => Array.Empty<object>()).HasValue.Should().BeFalse();
        }

        [Fact]
        public void SomeNotEmptyReturnsSomeWheEnumerableIsNotEmpty()
        {
            1.SomeNotEmpty(i => Enumerable.Repeat(1, 1))
             .ValueOrFailure()
             .Should()
             .Be(1);
        }

        [Fact]
        public void SomeWhenNotNullReturnNoneWhenObjectIsNull()
        {
            ((object?) null).SomeWhenNotNull().HasValue.Should().BeFalse();
        }

        [Fact]
        public void SomeWhenNotNullReturnSomeWhenObjectIsNotNull()
        {
            "test".SomeWhenNotNull().ValueOrFailure().Should().Be("test");
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnNoneWithExceptionObjectWhenTargetObjectIsNull()
        {
            ((string?) null).SomeWhenNotNull(() => "exception").ExceptionOrFailure().Should().Be("exception");
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnSomeWhenTargetObjectIsNotNull()
        {
            "test".SomeWhenNotNull(() => "exception").ValueOrException().Should().Be("test");
        }
    }
}
using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void SomeNotEmptyEitherReturnsNoneWhenEnumerableIsEmpty()
        {
            1.SomeNotEmpty(arg => Array.Empty<object>(), o => o.ToString())
             .Should()
             .BeNone()
             .Which.Should()
             .Be("1");
        }

        [Fact]
        public void SomeNotEmptyEitherReturnsSomeWheEnumerableIsNotEmpty()
        {
            1.SomeNotEmpty(i => Enumerable.Repeat(1, 1), i => default(object))
             .Should()
             .HaveSome()
             .Which.Should()
             .Be(1);
        }

        [Fact]
        public void SomeNotEmptyReturnsNoneWhenEnumerableIsEmpty()
        {
            default(object).SomeNotEmpty(arg => Array.Empty<object>()).Should().BeNone();
        }

        [Fact]
        public void SomeNotEmptyReturnsSomeWheEnumerableIsNotEmpty()
        {
            1.SomeNotEmpty(i => Enumerable.Repeat(1, 1))
             .Should()
             .HaveSome()
             .Which.Should()
             .Be(1);
        }

        [Fact]
        public void SomeWhenNotNullReturnNoneWhenObjectIsNull()
        {
            ((object?) null).SomeWhenNotNull().Should().BeNone();
        }

        [Fact]
        public void SomeWhenNotNullReturnSomeWhenObjectIsNotNull()
        {
            "test".SomeWhenNotNull().Should().HaveSome().Which.Should().Be("test");
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnNoneWithExceptionObjectWhenTargetObjectIsNull()
        {
            ((string?) null).SomeWhenNotNull(() => "exception").Should().BeNone().Which.Should().Be("exception");
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnSomeWhenTargetObjectIsNotNull()
        {
            "test".SomeWhenNotNull(() => "exception").Should().HaveSome().Which.Should().Be("test");
        }
    }
}
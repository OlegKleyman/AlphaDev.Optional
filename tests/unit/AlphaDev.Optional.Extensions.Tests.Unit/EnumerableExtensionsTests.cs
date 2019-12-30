using System.Linq;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void SomeNotEmptyEitherReturnsNoneWhenEnumerableIsEmpty()
        {
            var exception = new object();
            var result = Enumerable.Empty<int>().SomeNotEmpty(() => exception);
            result.Should().BeNone().Which.Should().BeSameAs(exception);
        }

        [Fact]
        public void SomeNotEmptyEitherReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var target = new[] { 1 };
            var result = target.SomeNotEmpty(() => new object());
            result.Should().HaveSome().Which.Should().BeSameAs(target);
        }

        [Fact]
        public void SomeNotEmptyReturnsNoneWhenEnumerableIsEmpty()
        {
            Enumerable.Empty<int>().SomeNotEmpty().Should().BeNone();
        }

        [Fact]
        public void SomeNotEmptyReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var target = new[] { 1 };
            var result = target.SomeNotEmpty();
            result.Should().HaveSome().Which.Should().BeSameAs(target);
        }
    }
}